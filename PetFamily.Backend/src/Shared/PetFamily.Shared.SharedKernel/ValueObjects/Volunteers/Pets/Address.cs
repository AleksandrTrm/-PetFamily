using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record Address
{
    private const int MAX_HOUSE_TITLE_LENGTH = 4;
    private const string HOUSE_NUMBER_REGEX = @"^\d{1,3}\d{0,1}[А-Г]?$";
    
    private Address(
        string district, 
        string settlement, 
        string street, 
        string house)
    {
        District = district;
        Settlement = settlement;
        Street = street;
        House = house;
    }

    public string District { get; }

    public string Settlement { get; }

    public string Street { get; }
    
    public string House { get; }

    public static Result<Address, Error.Error> Create(
        string district, 
        string settlement, 
        string street, 
        string house)
    {
        
        if (string.IsNullOrWhiteSpace(district))
            return Errors.General.InvalidValue(nameof(district));

        if (district.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(district));
        
        if (string.IsNullOrWhiteSpace(settlement))
            return Errors.General.InvalidValue(nameof(settlement));

        if (settlement.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(settlement));
        
        
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.InvalidValue(nameof(street));

        if (street.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(street));
        
        if (string.IsNullOrWhiteSpace(house))
            return Errors.General.InvalidValue(nameof(house));

        if (house.Length > MAX_HOUSE_TITLE_LENGTH)
            return Errors.General.InvalidLength(MAX_HOUSE_TITLE_LENGTH, nameof(house));

        if (Regex.IsMatch(house, HOUSE_NUMBER_REGEX))
            return Errors.General.InvalidValue(nameof(house));

        return new Address(district, settlement, street, house);
    }
}