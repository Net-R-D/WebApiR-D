using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RoomBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsBusiness _bookingsBusiness;
        private readonly IRoomsBusiness _roomBusiness;

        public BookingsController(IBookingsBusiness bookingsBusiness, IRoomsBusiness roomsBusiness)
        {
            _bookingsBusiness = bookingsBusiness;
            _roomBusiness = roomsBusiness;
        }

        /// <summary>
        /// Book a room
        /// </summary>
        /// <param name="booking">The new booking</param>
        /// <remarks>
        ///     - <b>booking :</b>   
        ///     { <br />
        ///        <b> "Room":</b>The room<br />
        ///        <b> "User":</b>The user<br />
        ///        <b> "StartSlot":</b>The start slot between 1 and 24<br />
        ///        <b> "EndSlot":</b>The end slot between 1 and 24 and greater than the start slot<br />
        ///        <b> "Date":</b>The date of the booking
        ///     }        
        /// </remarks>
        /// <returns>The created booking or a list of available rooms</returns>
        [HttpPost]
        public async Task<IActionResult> BookARoomAsync([FromBody] Booking booking)
        {
            IEnumerable<Room> rooms = await _roomBusiness.GetRoomsAsync();

            if (!ModelState.IsValid ||
                rooms.All(r => r.Name != booking.Room.Name))
            {
                return BadRequest();
            }

            CreatedBooking createdBooking = await _bookingsBusiness.BookARoomAsync(booking);

            if (createdBooking.Booking == null)
            {
                return Ok(createdBooking);
            }

            return CreatedAtAction("BookARoomAsync", createdBooking);
        }

        /// <summary>
        /// Delete a booking
        /// </summary>
        /// <param name="id">The id of booking</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteABookAsync(int id)
        {
            Booking booking = await _bookingsBusiness.DeleteABookingAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}