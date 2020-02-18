using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsBusiness _roomsBusiness;

        public RoomsController(IRoomsBusiness roomsBusiness)
        {
            _roomsBusiness = roomsBusiness;
        }

        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns>All rooms</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoomsAsync()
        {
            IEnumerable<Room> rooms = await _roomsBusiness.GetRoomsAsync();
            return Ok(rooms);
        }
    }
}