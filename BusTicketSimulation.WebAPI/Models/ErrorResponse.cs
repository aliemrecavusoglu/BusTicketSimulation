namespace BusTicketSimulation.WebAPI.Models
{
    public class ErrorResponse
    {
        public int StatusCode {  get; set; }    //Http kodu
        public string Message { get; set; } = string.Empty;     //Kullanıcının göreceği mesaj
        public string DetailedMessage { get; set; } = string.Empty;     //Bizim göreceğimiz mesaj
    }
}
