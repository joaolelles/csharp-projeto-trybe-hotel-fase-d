using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            var getCities = _context.Cities.Select(City => new CityDto
            {
                CityId = City.CityId,
                Name = City.Name,
                State = City.State,
            });

            return getCities;
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            var postCities = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State,
            };
            return postCities;
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var putCity = _context.Cities.FirstOrDefault(c => c.CityId == city.CityId);

            if (putCity == null)
            {
                return null!;
            }
            putCity.Name = city.Name;
            putCity.State = city.State;

            _context.Cities.Update(putCity);
            _context.SaveChanges();
            var updatedCity = new CityDto
            {
                CityId = putCity.CityId,
                Name = putCity.Name,
                State = putCity.State,
            };
            return updatedCity;
        }

    }
}