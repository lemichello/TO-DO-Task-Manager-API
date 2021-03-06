namespace DTO
{
    public class ProjectDto : BaseDto
    {
        // If Id is null, this is default project (Inbox, Today, Upcoming, Logbook).
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}