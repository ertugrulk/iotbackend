using System;
using System.Net;
using FluentValidation;
using IoTBackend.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IoTBackend.API.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        private (HttpStatusCode, string) GetResponseFromException(Exception ex)
        {
            return ex switch
            {
                DeviceNotFoundException _ => (HttpStatusCode.NotFound, "Device not found"),
                ValidationException _ => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, ex.Message)
            };
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception != null)
            {
                var (code, message) = GetResponseFromException(context.Exception);
                context.Result = new ObjectResult(message)
                {
                    StatusCode = (int)code
                };
                context.ExceptionHandled = true;
            }
        }
    }
}