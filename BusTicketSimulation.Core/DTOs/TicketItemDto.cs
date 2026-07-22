using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.DTOs
{
    public class TicketItemDto
    {
        public int SeatNumber { get; set; }
        public string Gender { get; set; } = string.Empty; 
    }
}
