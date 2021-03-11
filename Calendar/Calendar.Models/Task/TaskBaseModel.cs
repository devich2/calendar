using System;

namespace Calendar.Models.Task
{
    public class TaskBaseModel
    {
        public string Text { get; set; }
        public DateTimeOffset DeadLine { get; set; }
    }
}