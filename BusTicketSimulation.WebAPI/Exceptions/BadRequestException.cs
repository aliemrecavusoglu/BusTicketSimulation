namespace BusTicketSimulation.WebAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)  //Hata mesajını üst sınıfa(exception) yollar
        {
            
        }
    }
}
