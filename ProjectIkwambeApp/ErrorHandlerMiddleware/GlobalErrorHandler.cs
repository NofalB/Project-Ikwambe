using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using ProjectIkwambe.Utils;
using SendGrid.Helpers.Errors.Model;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ProjectIkwambe.ErrorHandlerMiddleware
{
    public class GlobalErrorHandler : IFunctionsWorkerMiddleware
    {
        ILogger Logger { get; }
        public GlobalErrorHandler(ILogger<GlobalErrorHandler> Logger)
        {
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error found in endpoint {context.FunctionDefinition.Name}: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(FunctionContext context, Exception exception)
        {
            var req = context.GetHttpRequestData();
            
            HttpResponseData response = req.CreateResponse();

            var responseData = new
            {
                Status = exception switch
                {
                    NotFoundException => HttpStatusCode.NotFound,
                    BadRequestException => HttpStatusCode.BadRequest,
                    ArgumentNullException => HttpStatusCode.BadRequest,
                    NullReferenceException => HttpStatusCode.BadRequest,
                    FileNotFoundException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                },
                Message = exception.Message
            };

            response.StatusCode = responseData.Status;

            await response.WriteAsJsonAsync(responseData);

            context.InvokeResult(response);
        }
    }
}
