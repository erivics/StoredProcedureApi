using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using StoredProcedureApi.Exceptions;
using UnauthorizedAccessException = StoredProcedureApi.Exceptions.UnauthorizedAccessException;

namespace StoredProcedureApi.Utility
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                
                await HandleExceptionAsync(context, ex);
            }
           
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = String.Empty;
            string message;
            var exceptionType = exception.GetType();
            if(exceptionType == typeof(BadRequestException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if(exceptionType == typeof(NotFoundException))
            {
                status = HttpStatusCode.NotFound;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if(exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            
            var exceptionResult = JsonSerializer.Serialize(new {error = message, stackTrace});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}