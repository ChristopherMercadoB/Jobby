using Jobby.Core.Application.Exceptions;
using Jobby.Core.Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace Jobby.Presentation.WebApi.Middlewares
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
            catch (Exception e)
            {

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeded = false, Message = e.Message };
                switch (e)
                {
                    case ApiException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case ValidationException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = ex.Errors;
                        break;

                    case KeyNotFoundException ex:
                        response.StatusCode = (int)HttpStatusCode.NotFound;

                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
