using AutoMapper;
using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.WebAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]     //kontrolcüye erişilecek internet adresi
    [ApiController] 
    public class BusController : ControllerBase     
    {
        private readonly IBusRepository busRepository;
        private readonly IMapper mapper;

        //Dependency Injection(Bağımlılık Enjeksiyonu): Veritabanı arayüzünü ve AutoMapper kütüphanesini sisteme dahil ediyoruz
        public BusController(IBusRepository bRepository, IMapper htmlMapper)
        {
            busRepository = bRepository;
            mapper = htmlMapper;
        }

        [HttpGet]
        public async Task<IActionResult>GetAll()
        {

            var buses = await busRepository.GetAllAsync();
            return Ok(buses);   //Http 200 (yani başarılı) döner
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(Guid id)
        {
            var bus = await busRepository.GetByIdAsync(id);
            if (bus == null)
            {
                throw new NotFoundException($"{id} kimlik numaralı otobüs sistemde mevcut değil ❌");    //Http 404 hatası döner
            }
            return Ok(bus);
        }

        [HttpPost]
        public async Task<IActionResult>Create(BusCreateDto dto)
        {
            var bus = mapper.Map<Bus>(dto);

            await busRepository.AddAsync(bus);
            await busRepository.SaveChangesAsync();
            return Ok("Otobüs başarıyla eklendi ✅");     //Http 200 (yani başarılı) döner
        }
    }
}
