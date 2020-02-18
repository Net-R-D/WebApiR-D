using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RoomBooking.Api.Controllers;
using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;

namespace RoomBooking.Tests.Api
{
    public class RoomsControllerTest
    {
        [Test]
        public async Task Shoud_Get_All_Rooms()
        {
            var fixture = new Fixture();
            var rooms = fixture.CreateMany<Room>();
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(rooms);
            var roomsController = new RoomsController(roomsBusiness);
            IActionResult response = await roomsController.GetRoomsAsync();

            Assert.IsInstanceOf(typeof(OkObjectResult), response);
            var objectResult = (OkObjectResult)response;
            Assert.AreEqual(200, objectResult.StatusCode);
            var roomFromController = (IEnumerable<Room>)objectResult.Value;
            Assert.AreEqual(rooms.Count(), roomFromController.Count());
        }
    }
}
