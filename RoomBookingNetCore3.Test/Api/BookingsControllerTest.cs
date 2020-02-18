using System.Collections.Generic;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RoomBooking.Api.Controllers;
using RoomBooking.Business.Interfaces;
using RoomBooking.Common.Models;
using System.Threading.Tasks;

namespace RoomBooking.Tests.Api
{
    public class BookingsControllerTest
    {
        Fixture fixture;

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
        }

        [Test]
        public async Task Shoud_Book_A_Room()
        {
           // var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 5,
                Room = fixture.Create<Room>()
            };
            var createdBookingForSubstitute = new CreatedBooking
            {
                Booking = new Booking
                {
                    Id = 1,
                    User = booking.User,
                    StartSlot = booking.StartSlot,
                    EndSlot = booking.EndSlot,
                    Room = booking.Room
                }
            };
            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            bookingsBusiness.BookARoomAsync(Arg.Any<Booking>()).Returns(createdBookingForSubstitute);
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(new List<Room> { booking.Room });
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            IActionResult response = await bookingsController.BookARoomAsync(booking);

            Assert.IsInstanceOf(typeof(CreatedAtActionResult), response);
            var objectResult = (CreatedAtActionResult)response;
            Assert.AreEqual(201, objectResult.StatusCode);
            var bookingFromController = (CreatedBooking)objectResult.Value;
            Assert.Less(0, bookingFromController.Booking.Id);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_When_Unvalid_Data()
        {
            //var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 3,
                Room = fixture.Create<Room>()
            };

            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(new List<Room> { booking.Room });
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            bookingsController.ModelState.AddModelError("", "");
            IActionResult response = await bookingsController.BookARoomAsync(booking);

            Assert.IsInstanceOf(typeof(BadRequestResult), response);
            var objectResult = (BadRequestResult)response;
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_When_Room_Already_Book()
        {
            //var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 3,
                Room = fixture.Create<Room>()
            };
            var createdBooking = new CreatedBooking
            {
                AvaialbleBookingsForDateAndRoom = fixture.CreateMany<Booking>(10)
            };

            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            bookingsBusiness.BookARoomAsync(Arg.Any<Booking>()).Returns(createdBooking);
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(new List<Room> { booking.Room });
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            IActionResult response = await bookingsController.BookARoomAsync(booking);

            Assert.IsInstanceOf(typeof(OkObjectResult), response);
            var objectResult = (OkObjectResult)response;
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task Shoud_Delete_A_Book()
        {
            //var fixture = new Fixture();
            var booking = fixture.Build<Booking>()
                .With(b => b.Id, 1)
                .Create();
            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            bookingsBusiness.DeleteABookingAsync(1).Returns(booking);
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(new List<Room> { booking.Room });
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            IActionResult response = await bookingsController.DeleteABookAsync(1);

            Assert.IsInstanceOf(typeof(OkResult), response);
            var objectResult = (OkResult)response;
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task Shoud_Return_404_When_Book_Does_Not_Exist()
        {
            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            bookingsBusiness.DeleteABookingAsync(1).Returns(Task.FromResult<Booking>(null));
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            IActionResult response = await bookingsController.DeleteABookAsync(1);

            Assert.IsInstanceOf(typeof(NotFoundResult), response);
            var objectResult = (NotFoundResult)response;
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_When_Room_Does_Not_Exist()
        {
            //var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 3,
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room50")
                    .Create()
            };
            var rooms = fixture.Build<Room>()
                .With(r => r.Name, "room4")
                .CreateMany(1);

            var bookingsBusiness = Substitute.For<IBookingsBusiness>();
            var roomsBusiness = Substitute.For<IRoomsBusiness>();
            roomsBusiness.GetRoomsAsync().Returns(rooms);
            var bookingsController = new BookingsController(bookingsBusiness, roomsBusiness);
            IActionResult response = await bookingsController.BookARoomAsync(booking);

            Assert.IsInstanceOf(typeof(BadRequestResult), response);
            var objectResult = (BadRequestResult)response;
            Assert.AreEqual(400, objectResult.StatusCode);
        }
    }
}
