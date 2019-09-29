namespace DAL.Entities
{
    public class ItemTags
    {
        public int ToDoItemId { get; set; }
        
        public int TagId { get; set; }

        public ToDoItem ToDoItem { get; set; }
        public Tag Tag { get; set; }
    }
}
