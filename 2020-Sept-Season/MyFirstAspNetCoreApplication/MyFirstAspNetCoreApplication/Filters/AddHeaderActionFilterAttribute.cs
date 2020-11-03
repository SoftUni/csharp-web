using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MyFirstAspNetCoreApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Filters
{
    public class AddHeaderActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IShortStringService shortStringService;

        public AddHeaderActionFilterAttribute(IShortStringService shortStringService)
        {
            this.shortStringService = shortStringService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("X-Info-Action-Name", context.ActionDescriptor.DisplayName);
        }

        /* public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before
            if (...)
            {
                next();
            }
            // after
        } */

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
