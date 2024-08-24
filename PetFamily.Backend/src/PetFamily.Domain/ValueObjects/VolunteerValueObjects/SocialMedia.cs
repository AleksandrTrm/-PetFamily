using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects
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

        public static Result<SocialMedia, string> Create(string title, string link)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "Title can not be empty";

            if (title.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return "The count of characters for title can not" +
                       $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
            
            if (string.IsNullOrWhiteSpace(link))
                return "Link can not be empty";

            if (link.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
                return "The count of characters for link can not" +
                       $" be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

            return new SocialMedia(title, link);
        }
    }
}
