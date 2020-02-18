using RoomBooking.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Dal.Interfaces
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<IEnumerable<Booking>> GetBookingsByDateAndRoomAsync(DateTime date, string roomName);
    }
}
