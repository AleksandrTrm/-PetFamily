using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record Address
{
    private Address()
    {
    }
    
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

    public static Result<Address, string> Create(string oblast, string district, string settlement, string street, string house)
    {
        if (string.IsNullOrWhiteSpace(oblast))
            return "Oblast can not be empty";

        if (oblast.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return "The count of characters for oblast title can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
        
        
        if (string.IsNullOrWhiteSpace(district))
            return "District can not be empty";

        if (district.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return "The count of characters for district title can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
        
        
        if (string.IsNullOrWhiteSpace(settlement))
            return "Settlement can not be empty";

        if (settlement.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return "The count of characters for settlement title can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
        
        
        if (string.IsNullOrWhiteSpace(street))
            return "Settlement can not be empty";

        if (street.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return "The count of characters for street title can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
        
        
        if (string.IsNullOrWhiteSpace(house))
            return "Settlement can not be empty";

        if (house.Length > Constants.MAX_HOUSE_TITLE_LENGTH)
            return "The count of characters for settlement title can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";

        if (house.Any(c => char.IsDigit(c) == false))
            return "House title must contains digits";

        return new Address(oblast, district, settlement, street, house);
    }
}