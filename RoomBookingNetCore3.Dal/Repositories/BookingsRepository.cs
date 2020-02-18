using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Dal.Repositories
{
    public class BookingsRepository : IBookingsRepository
    {
        public async Task<Booking> BookARoomAsync(Booking booking)
        {
            return await Task.Run(() =>
            {
                int identity = FakeDb.Bookings.Any() ? FakeDb.Bookings.Max(b => b.Id) : 0;
                booking.Id = identity + 1;
                FakeDb.Bookings = FakeDb.Bookings.Append(booking);
                return booking;
            });
        }

        public async Task<Booking> DeleteABookingAsync(int id)
        {
            return await Task.Run(() =>
            {
                Booking booking = FakeDb.Bookings.SingleOrDefault(b => b.Id == id);
                FakeDb.Bookings = FakeDb.Bookings.Where(b => b.Id != id).ToList();
                return booking;
            });

        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAndRoomAsync(DateTime date, string roomName)
        {
            return await Task.Run(() => FakeDb.Bookings.Where(b => b.Date.Date == date.Date && b.Room.Name == roomName));
        }
    }
}
