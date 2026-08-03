using AutoMapper;
using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Security.Claims;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]     //kontrolcüye erişilecek internet adresi
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;

        public TripController(ITripRepository tripRepository, IMapper htmlMapper)
        {
            _tripRepository = tripRepository;
            _mapper = htmlMapper;
        }

        [HttpGet]
        [AllowAnonymous]    //Sefer listesini çekmek için token zorunlu olmasın
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripRepository.GetAllAsync();
            var activeTrips = trips.Where(t => t.DepartureTime >= DateTime.Now).ToList();   //Sadece kalkış zamanı henüz gelmemiş seferleri listele
            var result = _mapper.Map<IEnumerable<TripResultDto>>(activeTrips); //Ham veritabanı modellerini Vue tarafının anlayacağı TripResultDto listesine dönüştürüyoruz
            return Ok(result);   //Http 200
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTrips([FromQuery] string from, [FromQuery] string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return BadRequest("Kalkış ve varış yerleri boş bırakılamaz❗");   //Http 400 hatası döner

            var trips = await _tripRepository.GetTripsByRouteAsync(from, to);
            var activeTrips = trips.Where(t => t.DepartureTime >= DateTime.Now).ToList();   //Sadece kalkış zamanı henüz gelmemiş seferleri listele
            var result = _mapper.Map<IEnumerable<TripResultDto>>(activeTrips);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TripCreateDto dto)
        {
            var trip = _mapper.Map<Trip>(dto);
            await _tripRepository.AddAsync(trip);
            await _tripRepository.SaveChangesAsync();

            return Ok("Sefer başarıyla oluşturuldu ✅");   // Http 200
        }

        //Toplu bilet alma
        [HttpPost("buy-tickets-bulk")]
        [Authorize]     //Token doğrulaması yapar
        public async Task<IActionResult> BuyTicketsBulk([FromBody] BulkTicketDto dto)
        {
            //JWT Token içinden giriş yapan kullanıcının ID'sini okuyoruz
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token içinde kullanıcı kimliği bulunamadı❗");
            }

            Guid currnetUserId = Guid.Parse(userIdClaim);

            var trip = await _tripRepository.GetByIdAsync(dto.TripId);
            if (trip == null) return NotFound("Sefer bulunamadı❗");

            //Sadece önceden veritabanına kaydedilmiş (kesin satılmış) koltukları çekiyoruz
            var dbSoldSeats = trip.SoldSeats.ToList();

            //Sepetteki biletleri tek tek kontrol et
            foreach (var ticketDto in dto.Tickets)
            {
                //Çift rezervasyon kontrolü
                //Eğer bu koltuk numarası veritabanından çektiğimiz dbSoldSeats listesinde zaten varsa direkt engelle
                var alreadySold = dbSoldSeats.Any(s => s.SeatNumber == ticketDto.SeatNumber);
                if (alreadySold)
                {
                    return BadRequest($"{ticketDto.SeatNumber} numaralı koltuk zaten başka bir yolcu tarafından satın alınmış❗");
                }

                //Gelen veriyi temizle, büyük harfe çevir
                string busType = trip.Bus?.BusType?.Replace(" ", "").ToUpper() ?? "2+2";

                //Eğer gelen veri "2+2" veya "2+1" değilse, sistemi doğrudan varsayılana ("2+2") eşitle!
                if (busType != "2+2" && busType != "2+1")
                {
                    busType = "2+2";
                }

                //Yan koltuk numarasını hesapla
                int neighborSeatNumber = 0;

                if (busType == "2+2")
                {
                    neighborSeatNumber = (ticketDto.SeatNumber % 2 == 1) ? ticketDto.SeatNumber + 1 : ticketDto.SeatNumber - 1;
                }
                else if (busType == "2+1")
                {
                    if (ticketDto.SeatNumber % 3 == 1)
                    {
                        neighborSeatNumber = 0;
                    }
                    else
                    {
                        neighborSeatNumber = (ticketDto.SeatNumber % 3 == 2) ? ticketDto.SeatNumber + 1 : ticketDto.SeatNumber - 1;
                    }
                }

                //Eğer geçerli bir yan koltuk numarası belirlendiyse
                if (neighborSeatNumber > 0)
                {
                    //Yan koltuğu sadece veritabanındaki (önceden alınmış) koltuklar arasında arıyoruz
                    //Aynı sepette gelen diğer biletleri bu filtreye dahil etmiyoruz
                    var dbNeighborSeat = dbSoldSeats.FirstOrDefault(s => s.SeatNumber == neighborSeatNumber);

                    //Eğer yan koltuk önceden veritabanına kaydedilmişse ve cinsiyeti şu an alınandan farklıysa
                    if (dbNeighborSeat != null && dbNeighborSeat.Gender != ticketDto.Gender)
                    {
                        return BadRequest($"{neighborSeatNumber} numaralı koltuk daha önce farklı bir cinsiyet ({dbNeighborSeat.Gender}) tarafından satın alındığı için, yanındaki {ticketDto.SeatNumber} numaralı koltuğu bu cinsiyetle satın alamazsınız❗");
                    }
                }
            }

            //PNR kodunu backend'de üretiyoruz 
            //Guid.NewGuid() kullanarak tamamen benzersiz bir şifre üretiyor ve ilk 5 hanesini alıyoruz
            string generatedPnr = "TR-" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper(); 

            //Eğer tüm sepetteki biletler yukarıdaki "önceden alınmış yabancı koltuk" testinden geçtiyse, veritabanına kaydet:
            foreach (var ticketDto in dto.Tickets)
            {
                var newSoldSeat = new SoldSeat
                {
                    TripId = dto.TripId,
                    SeatNumber = ticketDto.SeatNumber,
                    Gender = ticketDto.Gender,
                    UserId = currnetUserId,
                    FirstName = ticketDto.FirstName,
                    LastName = ticketDto.LastName,
                    TcIdentity = ticketDto.TcIdentity,
                    Phone = ticketDto.Phone,
                    PnrNumber = generatedPnr
                };
                await _tripRepository.AddSoldSeatAsync(newSoldSeat);
            }

            await _tripRepository.SaveChangesAsync();
            return Ok(new { Message = "Seçtiğiniz tüm biletler başarıyla onaylandı ve satın alındı. 🎫", NewSeats = dto.Tickets });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] //Sadece admin rolüne sahip kullanıcılar güncelleme yapabilir
        public async Task<IActionResult> Update(Guid id, [FromBody] TripUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("URL'deki ID ile gönderilen ID bilgisi uyuşmuyor❗");
            }

            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null) return NotFound("Sefer bulunamadı❗");

            _mapper.Map(dto, trip);

            _tripRepository.Update(trip);
            await _tripRepository.SaveChangesAsync();

            return Ok("Sefer başarıyla güncellendi ✅");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null) return NotFound("Sefer bulunamadı❗");

            if (trip.SoldSeats != null && trip.SoldSeats.Any())
            {
                return BadRequest("Bu sefere ait satılmış biletler bulunmaktadır, doğrudan silinemez❗");
            }

            _tripRepository.Delete(trip);
            await _tripRepository.SaveChangesAsync();

            return Ok("Sefer başarıyla silindi ✅");
        }
    }
}