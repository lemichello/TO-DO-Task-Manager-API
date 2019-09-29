namespace DAL.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }

        public User User { get; set; }
        public Project Project { get; set; }
    }
}
