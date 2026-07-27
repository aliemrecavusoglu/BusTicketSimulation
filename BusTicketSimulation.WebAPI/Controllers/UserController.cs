using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;   
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]     //Kullanıcı işlemleri sadece oturum açmış kullanıcılara özeldir
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Kullanıcı profilini getiren endpoint
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Oturum bilgisi bulunamadı❗");
            }

            Guid userId = Guid.Parse(userIdClaim);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı❗");
            }

            var profileDto = new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return Ok(profileDto);
        }

        //Profile güncelleme endpointi
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Oturum bilgisi bulunamadı❗");
            }

            Guid userId = Guid.Parse(userIdClaim);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı❗");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new { message = "Profil başarıyla güncellendi. ✅" });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Oturum bilgisi bulunamadı❗");
            }

            Guid userId = Guid.Parse(userIdClaim);
            bool result = await _userRepository.ChangePasswordAsync(userId, dto);

            if (!result)
            {
                return BadRequest("Şifre değiştirme işlemi başarısız oldu. Mevcut şifrenizi kontrol edin. ❌");
            }

            return Ok(new { message = "Şifreniz başarıyla değiştirildi. ✅" });
        }
    }
}
