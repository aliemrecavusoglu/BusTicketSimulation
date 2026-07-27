using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.DTOs
{
    public class UserTicketResponseDto
    {
        public Guid TicketId { get; set; }
        public int SeatNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }
        public string BusPlateNumber { get; set; } = string.Empty;
    }
}
