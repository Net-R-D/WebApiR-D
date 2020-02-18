using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Common.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public Room Room { get; set; }
        [Required]
        public User User { get; set; }
        [Required, Range(1, 24)]
        public int StartSlot { get; set; }
        [Required, Range(1, 24)]
        public int EndSlot { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndSlot > StartSlot)
            {
                yield return new ValidationResult($"The EndSlot field is greater than StartSlot field.",
                    new List<string> { nameof(EndSlot) });
            }
        }
    }
}
