namespace PetFamily.VolunteerRequestManagement.Domain.Models;

public class VolunteerInfo
{
    public VolunteerInfo(
        string name, 
        string surname, 
        string phoneNumber, 
        int experience, 
        string? patronymic = null)
    {
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Experience = experience;
        Patronymic = patronymic;
    }    
    
    public string Name { get; private set; }
    
    public string Surname { get; private set; }

    public string? Patronymic { get; private set; }

    public string PhoneNumber { get; private set; }

    public int Experience { get; private set; }
}