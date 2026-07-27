using BusTicketSimulation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<SoldSeat>> GetTicketsByUserIdAsync(Guid userId);
        Task AddSoldSeatAsync(SoldSeat soldSeat);
        Task<SoldSeat?> GetByIdAsync(Guid id);
        void DeleteSoldSeat(SoldSeat soldSeat);
        Task SaveChangesAsync();
    }
}
