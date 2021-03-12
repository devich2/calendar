using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Calendar.BLL.Abstract.Task;
using Calendar.Common;
using Calendar.DAL.Abstract;
using Calendar.DAL.Abstract.Transactions;
using Calendar.Entities.Tables;
using Calendar.Models.Response;
using Calendar.Models.Result;
using Calendar.Models.Task;
using Microsoft.Extensions.Logging;

namespace Calendar.BLL.Impl.Task
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<TaskService> _logger;

        public TaskService(IMapper mapper,
            IUnitOfWork unitOfWork,
            ITransactionManager transactionManager,
            ILogger<TaskService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task<DataResult<List<TaskGroupedModel>>> GetTaskListBetweenDates(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            List<ToDoTask> taskEntitiesList =
                (await _unitOfWork.ToDoTasks.GetAllAsync())  //To do, fix bug with GetByAsync
                .Where(x => x.DueDate.Date <= endDate.Date && x.DueDate.Date >= startDate.Date).ToList();

            if (taskEntitiesList.Count == 0)
            {
                return new DataResult<List<TaskGroupedModel>>()
                {
                    ResponseStatusType = ResponseStatusType.Warning,
                    Message = ResponseMessageType.EmptyResult
                };
            }

            List<TaskGroupedModel> result = taskEntitiesList
                .GroupBy(x => x.DueDate.Day)
                .Select(x => new {x.Key, L = x.ToList()})
                .Select(x =>
                {
                    var resultTasks = x.L.Select(_mapper.Map<TaskBlModel>).ToList();

                    return new TaskGroupedModel()
                    {
                        DayDate = resultTasks.First().DeadLine,
                        Result = resultTasks,
                        TotalCount = resultTasks.Count()
                    };
                }).ToList();
            
            return new DataResult<List<TaskGroupedModel>>()
            {
                ResponseStatusType = ResponseStatusType.Succeed,
                Data = result
            };
        }

        public async Task<DataResult<TaskAddResponse>> ProcessTaskCreate(TaskBaseModel model)
        {
            return await ProcessTaskCreateTransaction(model);
        }

        public async Task<DataResult<TaskUpdateResponse>> ProcessTaskUpdate(TaskBlModel model)
        {
            return await ProcessTaskUpdateTransaction(model);
        }

        public async Task<Result> ProcessTaskDelete(int id)
        {
            return await ProcessTaskDeleteTransaction(id);
        }
        
        private async Task<DataResult<TaskAddResponse>> ProcessTaskCreateTransaction(TaskBaseModel model)
        {
            return await _transactionManager.ExecuteInImplicitTransactionAsync(async () =>
            {
                ToDoTask taskEntity = _mapper.Map<ToDoTask>(model);
                await _unitOfWork.ToDoTasks.AddAsync(taskEntity);
                await _unitOfWork.SaveAsync();

                return new DataResult<TaskAddResponse>()
                {
                    Data = new TaskAddResponse()
                    {
                        TaskId = taskEntity.TaskId
                    },
                    ResponseStatusType = ResponseStatusType.Succeed
                };
            });
        }
        
        private async Task<DataResult<TaskUpdateResponse>> ProcessTaskUpdateTransaction(TaskBlModel model)
        {
            return await _transactionManager.ExecuteInTransactionAsync(async transaction =>
            {
                try
                {
                    ToDoTask taskEntity =
                        await _unitOfWork.ToDoTasks.GetByIdAsync(model.Id);

                    if (taskEntity == null)
                    {
                        return new DataResult<TaskUpdateResponse>()
                        {
                            ResponseStatusType = ResponseStatusType.Error,
                            Message = ResponseMessageType.IdIsMissing
                        };
                    }

                    if (!string.IsNullOrWhiteSpace(model.Text))
                    {
                        taskEntity.Text = model.Text;
                        taskEntity.DueDate = model.DeadLine;
                        await _unitOfWork.ToDoTasks.UpdateAsync(taskEntity);
                        await _unitOfWork.SaveAsync();

                        await transaction.CommitAsync();
                        
                        return new DataResult<TaskUpdateResponse>()
                        {
                            Data = new TaskUpdateResponse()
                            {
                                TaskId = model.Id
                            },
                            ResponseStatusType = ResponseStatusType.Succeed
                        };
                    }

                    await transaction.RollbackAsync();
                    return new DataResult<TaskUpdateResponse>()
                    {
                        Data = new TaskUpdateResponse()
                        {
                            TaskId = model.Id
                        },
                        Message = ResponseMessageType.InvalidModel,
                        ResponseStatusType = ResponseStatusType.Error
                    };
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogW(e, "ContentAdminBlModel: {C}", args:
                        new object[]
                        {
                            AsJsonFormatter.Create(model)
                        });

                    return new DataResult<TaskUpdateResponse>()
                    {
                        Data = new TaskUpdateResponse()
                        {
                            TaskId = model.Id
                        },
                        Message = ResponseMessageType.InvalidModel,
                        ResponseStatusType = ResponseStatusType.Error
                    };
                }
            });
        }
        
        private async Task<Result> ProcessTaskDeleteTransaction(int id)
        {
            if (id < 1)
            {
                return new Result
                {
                    Message = ResponseMessageType.InvalidId,
                    ResponseStatusType = ResponseStatusType.Error,
                    MessageDetails = "id could not be less or equal to 0"
                };
            }

            return await _transactionManager.ExecuteInImplicitTransactionAsync(async () =>
            {
                var itemToDelete = await _unitOfWork.ToDoTasks.GetByIdAsync(id);
                if (itemToDelete != null)
                {
                    await _unitOfWork.ToDoTasks.DeleteAsync(itemToDelete);
                    await _unitOfWork.SaveAsync();
                    return new Result
                    {
                        ResponseStatusType = ResponseStatusType.Succeed
                    };
                }

                return new Result
                {
                    Message = ResponseMessageType.NotFound,
                    ResponseStatusType = ResponseStatusType.Error,
                    MessageDetails = "Could not find menu item with given id"
                };
            });
        }
    }
}