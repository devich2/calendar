using System;
using Calendar.BLL.Abstract.Converter;
using Calendar.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.Web.Infrastructure.Extension
{
    public class StatusCodesFilter : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var objectResult = context.Result as ObjectResult;
            Result result = objectResult?.Value as Result;

            if (result == null)
            {
                return;
            }

            var statusConverter = context.HttpContext.RequestServices
                .GetRequiredService<IConverterService<int, ResponseMessageType>>();

            var statusCode = statusConverter.Convert(result.Message);

            context.HttpContext.Response.StatusCode = statusCode;
        }
    }
}