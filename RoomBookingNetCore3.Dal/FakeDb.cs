using RoomBooking.Common.Models;
using System.Collections.Generic;

namespace RoomBooking.Dal
{
    public class FakeDb
    {
        public static IEnumerable<Room> Rooms { get; set; } = new List<Room>
        {
            new Room { Id = 1, Name = "room0" },
            new Room { Id = 2, Name = "room1" },
            new Room { Id = 3, Name = "room2" },
            new Room { Id = 4, Name = "room3" },
            new Room { Id = 5, Name = "room4" },
            new Room { Id = 6, Name = "room5" },
            new Room { Id = 7, Name = "room6" },
            new Room { Id = 8, Name = "room7" },
            new Room { Id = 9, Name = "room8" },
            new Room { Id = 10, Name = "room9" }
        };
        public static IEnumerable<User> Users { get; set; } = new List<User>();
        public static IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
