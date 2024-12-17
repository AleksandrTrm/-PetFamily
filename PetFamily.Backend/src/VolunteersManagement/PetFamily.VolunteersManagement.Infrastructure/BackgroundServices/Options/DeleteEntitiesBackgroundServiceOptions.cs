    namespace PetFamily.VolunteersManagement.Infrastructure.BackgroundServices.Options;

    public class DeleteEntitiesBackgroundServiceOptions
    {
        public const string DELETE_ENTITIES_BACKGROUND_SERVICE_OPTIONS = "DeleteEntitiesBackgroundServiceOptions"; 
        
        public double DeletingEntitiesHoursDelay { get; init; }
        
        public double DeletedEntityDaysLifetime  { get; init; }
    }