using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Role { get; set; } = "User"; //Admin veya User
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // İlişki: Bir kullanıcının birden fazla satın aldığı bilet olabilir
        public ICollection<SoldSeat> SoldSeats { get; set; } = new List<SoldSeat>();
    }
}
