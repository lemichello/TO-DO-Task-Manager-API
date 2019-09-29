using System;
using System.Text.Json.Serialization;

namespace DTO
{
    public class ToDoItemDto : BaseToDoItemDto
    {
        public int Id { get; set; }
        public DateTime CompleteDate { get; set; }
        [JsonIgnore] public int UserId { get; set; }
    }
}