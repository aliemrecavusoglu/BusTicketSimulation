namespace BusTicketSimulation.Core.Entities;

public class Bus : BaseEntity   
{
    public string PlateNumber { get; set; } = string.Empty;
    public int SeatCount { get; set; }  
    public string BusType { get; set; }
}
