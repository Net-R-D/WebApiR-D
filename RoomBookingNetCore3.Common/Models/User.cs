using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
    }
}
