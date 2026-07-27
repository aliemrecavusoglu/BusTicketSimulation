using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
