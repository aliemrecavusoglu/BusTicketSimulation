using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Entities
{
    public class SoldSeat : BaseEntity
    {
        public int SeatNumber { get; set; }
        public string Gender { get; set; }
        
        // İlişki (Foreign Key): Bu koltuk hangi sefere ait?
        public Guid TripId { get; set; }

        // Navigation Property: EF Core'un bu satılan koltuk üzerinden Sefer bilgilerine kolayca erişmesini sağlar
        public Trip Trip { get; set; }
    }
}
