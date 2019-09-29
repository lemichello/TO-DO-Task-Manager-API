using System;

namespace DAL.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CompleteDate { get; set; }
        public int? ProjectId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Project Project { get; set; }
    }
}
