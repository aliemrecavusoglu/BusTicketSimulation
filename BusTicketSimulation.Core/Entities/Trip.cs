namespace BusTicketSimulation.Core.Entities;

public class Trip : BaseEntity
{
    public string From { get; set; } = string.Empty; //kalkış şehri
    public string To { get; set; } = string.Empty;   //varış şehri
    public DateTime DepartureTime { get; set; }      //kalkış saati
    public decimal Price { get; set; }               //bilet fiyatı

    //İlişki: Bu sefer hangi otobüsle yapılacak?
    public Guid BusId { get; set; }
    public Bus? Bus { get; set; } = null!;   //bus sınıfında ki verileri çekiyoruz
    public List<SoldSeat> SoldSeats { get; set; } = new List<SoldSeat>();   //Bir seferin birden fazla satılmış koltuk bilgisi olabilir (1'e Çok İlişki)
}