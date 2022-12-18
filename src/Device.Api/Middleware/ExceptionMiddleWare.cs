using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Device.Api.Middleware
{

    public class ExceptionMiddleware
    {
        private const string UNEXPECTED_ERROR = "Something really unexpected has happened!!";
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly RequestDelegate _request;
        public ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment webHostEnvironment)
        {
            _request = request;
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                _logger.LogError(e, UNEXPECTED_ERROR);
                await context.Response.WriteAsJsonAsync(new ProblemDetails()
                {
                    Status = 500,
                    Detail = webHostEnvironment.IsDevelopment() ? e.Message : UNEXPECTED_ERROR
                });
            }
        }
    }
}
