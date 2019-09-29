using System;

namespace DTO
{
    public class ToDoItemDto : BaseToDoItemDto
    {
        public int Id { get; set; }
        public DateTime CompleteDate { get; set; }
    }
}