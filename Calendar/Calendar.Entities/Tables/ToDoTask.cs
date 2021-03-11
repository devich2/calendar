using System;

namespace Calendar.Entities.Tables
{
    public class ToDoTask
    {
        public int TaskId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}