using System;
using System.Threading.Tasks;
using Calendar.DAL.Abstract;
using Calendar.DAL.Abstract.IRepository;
using Calendar.DAL.Impl.ImplRepository;

namespace Calendar.DAL.Impl
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly CalendarDbContext _calendarDbContext;
        private bool _disposed;
        public UnitOfWork(CalendarDbContext calendarDbContext)
        {
            _calendarDbContext = calendarDbContext;
        }


        #region Fields

        private ITaskRepository _taskRepository;
        #endregion

        #region Properties

        public ITaskRepository ToDoTasks
        {
            get
            {
                return _taskRepository ??= new TaskRepository(_calendarDbContext);
            }
        }

        #endregion

        #region Methods
        public Task<int> SaveAsync()
        {
            return _calendarDbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _calendarDbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
