using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateUserAsync(User user);
        Task<bool>ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
        Task SaveChangesAsync();
    }
}
