using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var getRooms = from room in _context.Rooms
                           join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                           join city in _context.Cities on hotel.CityId equals city.CityId
                           select new RoomDto
                           {
                               RoomId = room.RoomId,
                               Name = room.Name,
                               Capacity = room.Capacity,
                               Image = room.Image,
                               Hotel = new HotelDto
                               {
                                   HotelId = hotel.HotelId,
                                   Name = hotel.Name,
                                   Address = hotel.Address,
                                   CityId = city.CityId,
                                   CityName = city.Name,
                                   State = city.State
                               }
                           };
            return getRooms;
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            var postRooms = from r in _context.Rooms
                            join hotel in _context.Hotels on r.HotelId equals hotel.HotelId
                            join city in _context.Cities on hotel.CityId equals city.CityId
                            where r.RoomId == room.RoomId
                            orderby r.RoomId
                            select new RoomDto
                            {
                                RoomId = r.RoomId,
                                Name = r.Name,
                                Capacity = r.Capacity,
                                Image = r.Image,
                                Hotel = new HotelDto
                                {
                                    HotelId = hotel.HotelId,
                                    Name = hotel.Name,
                                    Address = hotel.Address,
                                    CityId = city.CityId,
                                    CityName = city.Name,
                                    State = city.State
                                }
                            };
            return postRooms.First();
        }

        public void DeleteRoom(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(room => room.RoomId == RoomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}