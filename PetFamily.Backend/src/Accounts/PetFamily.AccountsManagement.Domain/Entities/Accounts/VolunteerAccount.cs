﻿using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

namespace PetFamily.AccountsManagement.Domain.Entities.Accounts;

public class VolunteerAccount
{
    public Guid Id { get; set; }

    public int Experience { get; set; }

    public IReadOnlyList<Requisite> Requisites { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}