using System.Collections.Generic;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ICollection<ToDoItem> ToDoItems { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
