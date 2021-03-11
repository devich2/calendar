using Calendar.DAL.Abstract.IRepository;
using Calendar.DAL.Impl.ImplRepository.Base;
using Calendar.Entities.Tables;

namespace Calendar.DAL.Impl.ImplRepository
{
    public class TaskRepository : GenericKeyRepository<int, ToDoTask>, ITaskRepository
    {
        public TaskRepository(CalendarDbContext context) : base(context)
        {
        }
    }
}