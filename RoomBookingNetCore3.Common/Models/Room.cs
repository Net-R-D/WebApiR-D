using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Common.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
