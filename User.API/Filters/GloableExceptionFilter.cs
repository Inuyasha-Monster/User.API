using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace User.API.Filters
{
    public class GloableExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<GloableExceptionFilter> _logger;

        public GloableExceptionFilter(IHostingEnvironment hostingEnvironment, ILogger<GloableExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorReponse();
            if (context.Exception is UserOperationException)
            {
                json.Message = context.Exception.Message;
                json.DevelopMessage = context.Exception.StackTrace;
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "未知内部异常";
                if (_hostingEnvironment.IsDevelopment())
                {
                    json.DevelopMessage = context.Exception.StackTrace;
                }
                else
                {
                    json.DevelopMessage = context.Exception.Message;
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }
            _logger.LogError(context.Exception, context.Exception.Message);
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
