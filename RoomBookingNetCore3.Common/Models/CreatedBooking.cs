using System.Collections.Generic;

namespace RoomBooking.Common.Models
{
    public class CreatedBooking
    {
        public Booking Booking { get; set; }
        public IEnumerable<Booking> AvaialbleBookingsForDateAndRoom { get; set; }
    }
}
