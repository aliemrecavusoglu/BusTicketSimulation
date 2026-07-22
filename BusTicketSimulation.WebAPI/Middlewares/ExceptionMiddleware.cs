using BusTicketSimulation.WebAPI.Exceptions;
using BusTicketSimulation.WebAPI.Models;
using System.Net;
using System.Text.Json;

namespace BusTicketSimulation.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        //RequestDelegate: Bir sonraki adıma geçmeyi sağlar
        private readonly RequestDelegate next;
        
        public ExceptionMiddleware(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;    //Http 500 (sunucu hatası)
            var message = "Sunucuda beklenmedik hata oluştu❗";
            
            if(exception is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;     //Http 400
                message = exception.Message;
            }
            else if(exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;   //Http 404
                message = exception.Message;
            }
            else
            {
                message = "Sistemsel bir hata oluştu. Lütfen ekibimizle iletişime geçin❗";
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                DetailedMessage = exception.InnerException?.Message ?? exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);  //Json formatına dönüştürme
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
