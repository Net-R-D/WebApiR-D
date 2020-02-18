using System;
using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Dal.Repositories
{
    public class RoomsRepository : IRoomsRepository
    {
        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await Task.Run(() => FakeDb.Rooms);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAndRoomAsync(DateTime date, string roomName)
        {
            return await Task.Run(() => FakeDb.Bookings.Where(b => b.Date.Date == date.Date && b.Room.Name == roomName).ToList());
        }
    }
}
