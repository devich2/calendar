using System;
using System.Collections.Generic;
using AutoMapper;
using Calendar.DAL.Abstract;
using Calendar.Models;
using Calendar.Models.Response;
using Calendar.Models.Result;
using Calendar.Models.Task;

namespace Calendar.BLL.Abstract.Task
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<DataResult<List<TaskGroupedModel>>> GetTaskListBetweenDates(DateTimeOffset startDate, DateTimeOffset endDate);
        System.Threading.Tasks.Task<DataResult<TaskAddResponse>> ProcessTaskCreate(TaskBaseModel model);
        System.Threading.Tasks.Task<DataResult<TaskUpdateResponse>> ProcessTaskUpdate(TaskBlModel model);
        System.Threading.Tasks.Task<Result> ProcessTaskDelete(int id);
    }
}