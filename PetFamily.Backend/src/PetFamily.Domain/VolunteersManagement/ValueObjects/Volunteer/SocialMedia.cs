using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer
{
    public class SocialMedia
    {
        //ef core
        private SocialMedia()
        {
        }
        
        private SocialMedia(string title, string link)
        {
            Title = title;
            Link = link;
        }
        
        public string Title { get; }

        public string Link { get; }

        public static Result<SocialMedia, Error> Create(string title, string link)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.InvalidValue(nameof(title));

            if (title.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(title));
            
            if (string.IsNullOrWhiteSpace(link))
                return Errors.General.InvalidValue(nameof(link));

            if (link.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(link));

            return new SocialMedia(title, link);
        }
    }
}
