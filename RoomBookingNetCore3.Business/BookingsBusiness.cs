using System;
using System.Collections.Generic;
using System.Linq;
using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System.Threading.Tasks;

namespace RoomBooking.Business
{
    public class BookingsBusiness : IBookingsBusiness
    {
        private readonly IBookingsRepository _bookingsRepository;

        public BookingsBusiness(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }

        public async Task<CreatedBooking> BookARoomAsync(Booking booking)
        {
            IEnumerable<Booking> bookings = (await GetBookingsByDateAndRoomAsync(booking.Date, booking.Room.Name)).ToList();

            if (bookings.Any(b => b.EndSlot >= booking.StartSlot && b.EndSlot <= booking.EndSlot ||
                                  b.StartSlot >= booking.StartSlot && b.StartSlot <= booking.EndSlot))
            {
                var availableBookings = new List<Booking>();

                for (int i = 1; i <= 24; i++)
                {
                    if (!bookings.Any(b => i >= b.StartSlot && i <= b.EndSlot))
                    {
                        availableBookings.Add(new Booking
                        {
                            Room = booking.Room,
                            Date = booking.Date,
                            StartSlot = i,
                            EndSlot = i
                        });
                    }
                }

                return new CreatedBooking
                {
                    AvaialbleBookingsForDateAndRoom = availableBookings
                };
            }

            Booking newBooking = await _bookingsRepository.BookARoomAsync(booking);
            return new CreatedBooking
            {
                Booking = newBooking

            };
        }

        public async Task<Booking> DeleteABookingAsync(int id)
        {
            return await _bookingsRepository.DeleteABookingAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAndRoomAsync(DateTime date, string roomName)
        {
            return await _bookingsRepository.GetBookingsByDateAndRoomAsync(date, roomName);
        }
    }
}
