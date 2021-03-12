using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calendar.BLL.Abstract.Task;
using Calendar.Models.Response;
using Calendar.Models.Result;
using Calendar.Models.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calendar.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;


        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        [HttpGet]
        public async Task<DataResult<List<TaskGroupedModel>>> Get(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return await _taskService.GetTaskListBetweenDates(startDate, endDate);
        }
        [HttpPost]
        public async Task<DataResult<TaskAddResponse>> Create(TaskBlModel model)
        {
            return await _taskService.ProcessTaskCreate(model);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<DataResult<TaskUpdateResponse>> Update(int id, [FromBody] TaskBlModel model)
        {
            model.Id = id;
            return await _taskService.ProcessTaskUpdate(model);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<Result> Delete(int id)
        {
            return await _taskService.ProcessTaskDelete(id);
        }
    }
}