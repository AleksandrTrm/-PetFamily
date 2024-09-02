using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Update.UpdateRequisites;

public record UpdateRequisitesRequest(Guid VolunteerId, RequisitesDto Requisites);