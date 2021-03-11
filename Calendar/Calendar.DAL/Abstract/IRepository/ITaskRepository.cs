using Calendar.DAL.Abstract.IRepository.Base;
using Calendar.Entities.Tables;

namespace Calendar.DAL.Abstract.IRepository
{
    public interface ITaskRepository : IGenericKeyRepository<int, ToDoTask>
    {
    }
}