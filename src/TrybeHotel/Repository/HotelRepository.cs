using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        //  5. Refatore o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            var getHotels = from hotel in _context.Hotels
                            join city in _context.Cities on hotel.CityId equals city.CityId
                            select new HotelDto
                            {
                                HotelId = hotel.HotelId,
                                Name = hotel.Name,
                                Address = hotel.Address,
                                CityId = city.CityId,
                                CityName = city.Name,
                                State = city.State,
                            };


            return getHotels;
        }

        // 6. Refatore o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            var city = _context.Cities.FirstOrDefault(city => city.CityId == hotel.CityId);
            if (city != null)
            {
                var postHotels = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = city.CityId,
                    CityName = city.Name,
                    State = city.State,
                };
                return postHotels;
            }
            else
            {
                throw new ApplicationException("Cidade não encontrada.");
            }
        }
    }
}