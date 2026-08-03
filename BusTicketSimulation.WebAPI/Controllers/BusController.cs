using AutoMapper;
using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.WebAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]     //kontrolcüye erişilecek internet adresi
    [ApiController] 
    public class BusController : ControllerBase     
    {
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;

        //Dependency Injection(Bağımlılık Enjeksiyonu): Veritabanı arayüzünü ve AutoMapper kütüphanesini sisteme dahil ediyoruz
        public BusController(IBusRepository busRepository, IMapper mapper)
        {
            _busRepository = busRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetAll()
        {
            var buses = await _busRepository.GetAllAsync();
            return Ok(buses);   //Http 200 (yani başarılı) döner
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetById(Guid id)
        {
            var bus = await _busRepository.GetByIdAsync(id);
            if (bus == null)
            {
                throw new NotFoundException($"{id} kimlik numaralı otobüs sistemde mevcut değil ❌");    //Http 404 hatası döner
            }
            return Ok(bus);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Create(BusCreateDto dto)
        {
            var bus = _mapper.Map<Bus>(dto);

            await _busRepository.AddAsync(bus);
            await _busRepository.SaveChangesAsync();
            return Ok("Otobüs başarıyla eklendi ✅");     //Http 200 (yani başarılı) döner
        }
    }
}
