using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Calendar.Web.Infrastructure.Extension
{
    public class StatusCodeConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                controller.Filters.Add(new StatusCodesFilter());
            }
        }
    }
}