namespace BusTicketSimulation.WebAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)    //Hata mesajını üst sınıfa(exception) yollar
        {

        }
    }
}
