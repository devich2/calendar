using Calendar.BLL.Abstract.Converter;
using Calendar.BLL.Abstract.Task;
using Calendar.BLL.Impl.Converter;
using Calendar.BLL.Impl.Task;
using Calendar.Models.Result;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.BLL.Impl
{
    public class ServiceDependencyInstaller
    {
        public static void Install(IServiceCollection services)
        {
            //Task
            services.AddTransient<ITaskService, TaskService>();
            //Other dependencies
            services.AddSingleton<IConverterService<int, ResponseMessageType>, HttpStatusConverterService>();
        }
    }
}