using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.DTOs
{
    public class BulkTicketDto
    {
        public Guid TripId { get; set; }
        public List<TicketItemDto> Tickets { get; set; } = new List<TicketItemDto>();
    }
}
