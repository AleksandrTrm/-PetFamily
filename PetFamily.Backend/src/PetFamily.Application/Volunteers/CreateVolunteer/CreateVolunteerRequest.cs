﻿using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName, 
    DescriptionDto Description, 
    int Experience,
    PhoneNumberDto PhoneNumber, 
    IEnumerable<SocialMediaDto> SocialMedias, 
    IEnumerable<RequisiteDto> Requisites);