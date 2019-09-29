using System.Collections.Generic;

namespace DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
