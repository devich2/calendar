using AutoMapper;
using Calendar.Entities.Tables;
using Calendar.Models.Task;

namespace Calendar.BLL.Mappers
{
    public class TaskProfile: Profile
    {
        public TaskProfile()
        {
            CreateMap<ToDoTask, TaskBlModel>()
                .ForMember(x => x.DeadLine, options => options.MapFrom(src => src.DueDate))
                .ForMember(x => x.Id, options => options.MapFrom(src => src.TaskId));
            
            CreateMap<TaskBlModel, ToDoTask>()
                .ForMember(x => x.DueDate, options => options.MapFrom(src => src.DeadLine))
                .ForMember(x => x.TaskId, options => options.MapFrom(src => src.Id));
        }
    }
}