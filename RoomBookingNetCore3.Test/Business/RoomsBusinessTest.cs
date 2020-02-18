using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using RoomBooking.Business;
using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Tests.Business
{
    public class RoomsBusinessTest
    {
        [Test]
        public async Task Shoud_Get_All_Rooms()
        {
            var fixture = new Fixture();
            var rooms = fixture.CreateMany<Room>();
            var roomsRepository = Substitute.For<IRoomsRepository>();
            roomsRepository.GetRoomsAsync().Returns(rooms);
            var roomsBusiness = new RoomsBusiness(roomsRepository);
            IEnumerable<Room> roomsFromBusiness = await roomsBusiness.GetRoomsAsync();

            Assert.AreEqual(rooms.Count(), roomsFromBusiness.Count());
        }
    }
}
