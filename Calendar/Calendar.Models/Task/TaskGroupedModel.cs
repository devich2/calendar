using System;

namespace Calendar.Models.Task
{
    public class TaskGroupedModel : SelectionModel<TaskBlModel>

    {
        public DateTimeOffset DayDate { get; set; }
    }
}