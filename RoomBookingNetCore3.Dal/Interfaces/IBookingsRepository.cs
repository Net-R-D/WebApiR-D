using System;
using System.Collections.Generic;
using RoomBooking.Common.Models;
using System.Threading.Tasks;

namespace RoomBooking.Dal.Interfaces
{
    public interface IBookingsRepository
    {
        Task<Booking> BookARoomAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsByDateAndRoomAsync(DateTime date, string roomName);
        Task<Booking> DeleteABookingAsync(int id);
    }
}
