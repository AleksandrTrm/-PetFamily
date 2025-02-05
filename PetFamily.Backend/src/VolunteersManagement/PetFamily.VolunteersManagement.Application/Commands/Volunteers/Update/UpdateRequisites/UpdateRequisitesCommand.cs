﻿using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;