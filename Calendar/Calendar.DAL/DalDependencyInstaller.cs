using System;
using Calendar.DAL.Abstract;
using Calendar.DAL.Abstract.IRepository;
using Calendar.DAL.Abstract.Transactions;
using Calendar.DAL.Impl;
using Calendar.DAL.Impl.ImplRepository;
using Calendar.DAL.Impl.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.DAL
{
    public static class DalDependencyInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //Configure database context
            services.AddDbContext<CalendarDbContext>(options =>
            {
                options.UseSqlite(
                    connectionString,
                    p =>
                    {
                        p.MigrationsAssembly("Calendar.Web");
                    });
            });
            
            
            services.AddTransient<ITaskRepository, TaskRepository>();

            //Other dependencies
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionManager, DbTransactionManager>();
        }
    }
}
