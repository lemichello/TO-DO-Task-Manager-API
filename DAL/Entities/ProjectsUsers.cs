namespace DAL.Entities
{
    public class ProjectsUsers
    {
        public int ProjectId { get; set; }
        
        public int UserId { get; set; }
        
        public int InviterId { get; set; }

        public bool IsAccepted { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
        public User Inviter { get; set; }
    }
}
