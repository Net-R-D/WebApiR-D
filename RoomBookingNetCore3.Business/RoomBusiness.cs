using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Business
{
    public class RoomsBusiness : IRoomsBusiness
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsBusiness(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _roomsRepository.GetRoomsAsync();
        }
    }
}
