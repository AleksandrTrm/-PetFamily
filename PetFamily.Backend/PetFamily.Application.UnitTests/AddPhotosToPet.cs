using System.Collections;
using Moq;
using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Commands.Volunteers;
using PetFamily.Application.Features.Commands.Volunteers.Pet.UploadPetFiles;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.Entities.Pets;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.UnitTests;

public class AddPhotosToPet
{
    private readonly Mock<IVolunteersRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<UploadPetFilesHandler>> _loggerMock = new();
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IDbTransaction> _dbTransaction = new();
    private readonly Mock<IValidator<UploadPetFilesCommand>> _validatorMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<FileInfo>>> _messageQueueMock = new();

    [Fact]
    public async Task AddPhotosToPet_ShouldReturnSuccess()
    {
        //arrange
        var ct = new CancellationToken();
        
        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;

        var filesContents = CreateFileContent();
        
        var command = new UploadPetFilesCommand(
            volunteerId, 
            volunteer.Pets.First().Id.Value,
            CreateFilesCommands(filesContents));

        _repositoryMock.Setup(r => r.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransaction.Object);
        
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(ct))
            .Returns(Task.CompletedTask);

        _fileProviderMock.Setup(fp => fp.UploadFiles(filesContents, ct))
            .ReturnsAsync(Result.Success<IEnumerable<string>, Error>(filesContents
                .Select(f => f.FileInfo.Path)));

        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        var handler = new UploadPetFilesHandler(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object,
            _fileProviderMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AddPhotosToPet_ShouldReturnError_WhileValidateCommand()
    {
        //arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;
        
        var filesContents = CreateFileContent();

        var filesCommands = CreateFilesCommands(filesContents);

        var command = new UploadPetFilesCommand(volunteerId, volunteer.Pets.First().Id.Value, filesCommands);

        var validationError = Errors.General.InvalidValue(nameof(command.Files)).Serialize();
        var validationFailures = new List<ValidationFailure>()
        {
            new ValidationFailure(nameof(command.Files), validationError)
        };
        var validationResult = new ValidationResult(validationFailures);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);

        var handler = new UploadPetFilesHandler(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object,
            _fileProviderMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().InvalidField.Should().Be(nameof(command.Files));
    }

    [Fact]
    public async Task AddPhotosToPet_ShouldReturnError_WhenVolunteerNotFound()
    {
        //arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;

        var filesContent = CreateFileContent();

        var command = new UploadPetFilesCommand(
            volunteerId,
            volunteer.Pets.First().Id.Value,
            CreateFilesCommands(filesContent));

        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _repositoryMock.Setup(r => r.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Failure<Volunteer, Error>(Errors.General.NotFound(volunteerId)));

        var handler = new UploadPetFilesHandler(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object,
            _fileProviderMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        var error = result.Error.First();
        
        result.IsFailure.Should().BeTrue();
        error.Message.Should().Be($"Record not found for id - {volunteerId.Value}");
        error.Code.Should().Be("record.not.found");
    }

    [Fact]
    public async Task AddPhotosToPet_ShouldReturnError_WhileSavingPhotos()
    {
        //arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;

        var filesContents = CreateFileContent();
        
        var command = new UploadPetFilesCommand(
            volunteerId,
            volunteer.Pets.First().Id.Value,
            CreateFilesCommands([]));
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _repositoryMock.Setup(r => r.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransaction.Object);

        _fileProviderMock.Setup(fp => fp.UploadFiles(It.IsAny<List<FileContent>>(), ct))
            .ReturnsAsync(Result.Failure<IEnumerable<string>, Error>(
                Error.Failure("files.upload.error", "Failed to upload files to storage")));

        var loggerMock = new Mock<ILogger<UploadPetFilesHandler>>();
        
        var handler = new UploadPetFilesHandler(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object,
            _fileProviderMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object,
            loggerMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        var error = result.Error.First();
        
        result.IsFailure.Should().BeTrue();
        error.Message.Should().Be("Failed to upload files to storage");
        error.Code.Should().Be("files.upload.error");
    }
    
    private IEnumerable<FileContent> CreateFileContent()
    {
        var filesContents = new List<FileContent>()
        {
            new(new MemoryStream(), 
                new FileInfo(FilePath.Create(Guid.NewGuid().ToString() + ".jpg").Value, "photos")),
            
            new(new MemoryStream(), 
                new FileInfo(FilePath.Create(Guid.NewGuid().ToString() + ".jpg").Value, "photos"))
        };

        return filesContents;
    }
    
    private IEnumerable<UploadFileCommand> CreateFilesCommands(IEnumerable<FileContent> filesContents)
    {
        return filesContents.Select(fc => new UploadFileCommand(fc.Stream, fc.FileInfo.Path));
    }
    
    private Volunteer CreateVolunteerWithPets(int desiredPetCount)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("Name", "Surname", "Patronymic").Value,
            Description.Create("generalDescription").Value,
            5,
            PhoneNumber.Create("89000728412").Value,
            new ValueObjectList<SocialMedia>(new List<SocialMedia>()),
            new ValueObjectList<Requisite>(new List<Requisite>()));

        for (int i = 0; i < desiredPetCount; i++)
        {
            var pet = new Pet(
                PetId.NewPetId(),
                Nickname.Create($"Pet " + (i + 1)).Value,
                Guid.NewGuid(),
                BreedId.NewBreedId(), 
                Description.Create("generalDescription").Value,
                Color.Create("color").Value,
                HealthInfo.Create("healthInfo").Value,
                Address.Create("address", "address", "address", "1a").Value,
                10,
                10,
                PhoneNumber.Create("89000728412").Value,
                true,
                DateTime.Now,
                true,
                Status.LookingForHome,
                DateTime.Now,
                new List<Requisite>(),
                new List<PetPhoto>(new List<PetPhoto>()));

            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}