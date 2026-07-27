using AutoMapper;
using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]     //Bilet işlemleri sadece oturum açmış kullanıcılara özeldir
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        //Kullanıcının kendi biletlerini getirir
        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Oturum bilgisi doğrulanamadı❗");
            }

            Guid currentUserId = Guid.Parse(userIdClaim);

            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(currentUserId);
            var result = _mapper.Map<IEnumerable<UserTicketResponseDto>>(tickets);

            return Ok(result);   //Http 200
        }

        //Kullanıcının kendi biletini iptal etmesini sağlar
        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Oturum bilgisi doğrulanamadı❗");
            }

            Guid currentUserId = Guid.Parse(userIdClaim);

            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket == null)
            {
                return Forbid("Başka bir kullanıcıya ait bileti iptal edemezsiniz❗");
            }

            _ticketRepository.DeleteSoldSeat(ticket);
            await _ticketRepository.SaveChangesAsync();

            return Ok(new {Message = "Biletiniz başarıyla iptal edildi. ✅" });  //Http 204
        }
    }
}
