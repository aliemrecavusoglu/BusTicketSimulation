using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
        public Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }
        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            //Mevcut şifreyi doğrula
            if (!VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return false; // Mevcut şifre yanlış
            }

            CreatePasswordHash(dto.NewPassword, out byte[] newHash, out byte[] newSalt);

            //Yeni şifre için hash ve salt oluştur
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //Yardımcı metodlar
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
