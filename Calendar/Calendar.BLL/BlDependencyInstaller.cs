using Calendar.BLL.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.BLL
{
    public class BlDependencyInstaller
    {
        public static void Install(IServiceCollection services)
        {
            ServiceDependencyInstaller.Install(services);
            services.AddAutoMapper(typeof(BlDependencyInstaller));
        }
    }
}