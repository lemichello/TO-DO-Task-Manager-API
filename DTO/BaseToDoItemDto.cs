using System;

namespace DTO
{
    public class BaseToDoItemDto : BaseDto
    {
        public string Header { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
        public int? ProjectId { get; set; }
    }
}