using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (await _authRepository.UserExistsAsync(dto.Email))
                return BadRequest("Bu e-posta adresi zaten kullanımda❗");

            var user = await _authRepository.RegisterAsync(dto);
            return Ok(new { Message = "Kayıt başarıyla tamamlandı ✅" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authRepository.LoginAsync(dto);

            if (token == null)
                return BadRequest("E-posta veya şifre hatalı❗");

            return Ok(new { Token = token, Message = "Giriş başarılı 🔑" });
        }
    }
}