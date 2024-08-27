using System.Text.RegularExpressions;
using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record Address
{
    private const int MAX_HOUSE_TITLE_LENGTH = 4;
    private const string HOUSE_NUMBER_REGEX = @"^\d{1,3}\d{0,1}[А-Г]?$";
    
    private Address(string oblast, string district, string settlement, string street, string house)
    {
        Oblast = oblast;
        District = district;
        Settlement = settlement;
        Street = street;
        House = house;
    }
    
    public string Oblast { get; }

    public string District { get; }

    public string Settlement { get; }

    public string Street { get; }
    
    public string House { get; }

    public static Result<Address, Error> Create(string oblast, string district, string settlement, string street, string house)
    {
        if (string.IsNullOrWhiteSpace(oblast))
            return Errors.General.InvalidValue(nameof(oblast));

        if (oblast.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(oblast));
        
        
        if (string.IsNullOrWhiteSpace(district))
            return Errors.General.InvalidValue(nameof(oblast));

        if (district.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(district));
        
        
        if (string.IsNullOrWhiteSpace(settlement))
            return Errors.General.InvalidValue(nameof(oblast));

        if (settlement.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(settlement));
        
        
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.InvalidValue(nameof(oblast));

        if (street.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(street));
        
        if (string.IsNullOrWhiteSpace(house))
            return Errors.General.InvalidValue(nameof(oblast));

        if (house.Length > MAX_HOUSE_TITLE_LENGTH)
            return Errors.General.InvalidLength(MAX_HOUSE_TITLE_LENGTH, nameof(house));

        if (Regex.IsMatch(house, HOUSE_NUMBER_REGEX))
            return Errors.General.InvalidValue(nameof(house));

        return new Address(oblast, district, settlement, street, house);
    }
}