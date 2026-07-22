using BusTicketSimulation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Interfaces
{
    public interface IBusRepository
    {
        Task<Bus?> GetByIdAsync(Guid id);
        Task<IEnumerable<Bus>> GetAllAsync();
        Task AddAsync(Bus bus);
        void Update(Bus bus);
        void Delete(Bus bus);
        Task<bool> SaveChangesAsync();
    }
}
