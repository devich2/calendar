using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Calendar.DAL.Abstract.IRepository;

namespace Calendar.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        public ITaskRepository ToDoTasks { get; }
        public Task<int> SaveAsync();
    }
}