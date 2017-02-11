using System;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;

namespace Visma.TimeTracking.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException == null) throw;

                var inner = e.InnerException as PostgresException;
                if (inner?.SqlState == "23503") await HandleExceptionAsync(context, e, HttpStatusCode.NotFound);
                else await HandleExceptionAsync(context, e, HttpStatusCode.InternalServerError);
            }
            catch (ArgumentNullException e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.NotFound);
            }
            catch (InvalidOperationException e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            if (exception == null) return;

            await WriteExceptionAsync(context, exception, code).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                data = new []
                {
                    new
                    {
                        reason = exception.Message,
                        stackTrace = exception.ToString(),
                        exception = exception.GetType().Name
                    }
                }
            })).ConfigureAwait(false);
        }

    }
}