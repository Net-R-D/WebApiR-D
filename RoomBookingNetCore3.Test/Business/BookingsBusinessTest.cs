using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using RoomBooking.Business;
using RoomBooking.Common.Models;
using RoomBooking.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Tests.Business
{
    public class BookingsBusinessTest
    {
        [Test]
        public async Task Shoud_Book_A_Room()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 5,
                Room = fixture.Create<Room>()
            };
            var bookingForSubstitute = new Booking
            {
                Id = 1,
                User = booking.User,
                StartSlot = booking.StartSlot,
                EndSlot = booking.EndSlot,
                Room = booking.Room
            };
            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.BookARoomAsync(Arg.Any<Booking>()).Returns(bookingForSubstitute);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            Assert.Less(0, createdBookingFromBusiness.Booking.Id);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_If_Already_Booking()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 5,
                Date = new DateTime(2019, 3, 25),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };
            var bookings = fixture.Build<Booking>()
                .With(b => b.Date, new DateTime(2019, 3, 25))
                .With(b => b.Room, fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create())
                .CreateMany(10);
            bookings.ElementAt(4).StartSlot = 5;
            bookings.ElementAt(4).EndSlot = 7;

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(bookings);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.DidNotReceive().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNull(createdBookingFromBusiness.Booking);
        }

        [Test]
        public async Task Shoud_Book_A_Room_When_Same_Date_And_Slot_But_Another_Room()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 5,
                Date = new DateTime(2019, 3, 25),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(new List<Booking>(0));
            bookingsRepository.BookARoomAsync(Arg.Any<Booking>()).Returns(new Booking());
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.Received().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNotNull(createdBookingFromBusiness.Booking);
        }

        [Test]
        public async Task Shoud_Book_A_Room_When_Same_Room_And_Slot_But_Another_Date()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 5,
                EndSlot = 5,
                Date = new DateTime(2019, 3, 26),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 26), "room0").Returns(new List<Booking>(0));
            bookingsRepository.BookARoomAsync(Arg.Any<Booking>()).Returns(new Booking());
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.Received().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNotNull(createdBookingFromBusiness.Booking);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_When_Slot_Overlap_1()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 10,
                EndSlot = 14,
                Date = new DateTime(2019, 3, 25),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };
            var bookings = fixture.Build<Booking>()
                .With(b => b.Date, new DateTime(2019, 3, 25))
                .With(b => b.Room, fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create())
                .CreateMany(1);
            bookings.ElementAt(0).StartSlot = 8;
            bookings.ElementAt(0).EndSlot = 11;

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(bookings);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.DidNotReceive().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNull(createdBookingFromBusiness.Booking);
        }

        [Test]
        public async Task Shoud_Not_Book_A_Room_When_Slot_Overlap_2()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 10,
                EndSlot = 14,
                Date = new DateTime(2019, 3, 25),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };
            var bookings = fixture.Build<Booking>()
                .With(b => b.Date, new DateTime(2019, 3, 25))
                .With(b => b.Room, fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create())
                .CreateMany(1);
            bookings.ElementAt(0).StartSlot = 13;
            bookings.ElementAt(0).EndSlot = 16;

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(bookings);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.DidNotReceive().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNull(createdBookingFromBusiness.Booking);
        }

        [Test]
        public async Task ShoudGet_All_Booking_For_A_Date_And_A_Room()
        {
            var fixture = new Fixture();
            var bookings = fixture.Build<Booking>()
                .With(b => b.Date, new DateTime(2019, 3, 25))
                .With(b => b.Room, fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create())
                .CreateMany();
            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(bookings);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            IEnumerable<Booking> bookingsFromBusiness =
                await bookingsBusiness.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0");

            Assert.AreEqual(bookings.Count(), bookingsFromBusiness.Count());
            Assert.IsTrue(bookingsFromBusiness.All(b => b.Date == new DateTime(2019, 3, 25) && b.Room.Name == "room0"));
        }

        [Test]
        public async Task Shoud_Get_All_Available_Booking_When_Room_Is_Already_Booking()
        {
            var fixture = new Fixture();
            var booking = new Booking
            {
                User = fixture.Create<User>(),
                StartSlot = 10,
                EndSlot = 10,
                Date = new DateTime(2019, 3, 25),
                Room = fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create()
            };
            var bookings = fixture.Build<Booking>()
                .With(b => b.Date, new DateTime(2019, 3, 25))
                .With(b => b.Room, fixture.Build<Room>()
                    .With(r => r.Name, "room0")
                    .Create())
                .CreateMany(2);
            bookings.ElementAt(0).StartSlot = 5;
            bookings.ElementAt(0).EndSlot = 5;
            bookings.ElementAt(1).StartSlot = 10;
            bookings.ElementAt(1).EndSlot = 10;

            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.GetBookingsByDateAndRoomAsync(new DateTime(2019, 3, 25), "room0").Returns(bookings);
            var bookingsBusiness = new BookingsBusiness(bookingsRepository);
            CreatedBooking createdBookingFromBusiness = await bookingsBusiness.BookARoomAsync(booking);

            await bookingsRepository.DidNotReceive().BookARoomAsync(Arg.Any<Booking>());
            Assert.IsNull(createdBookingFromBusiness.Booking);
            Assert.AreEqual(22, createdBookingFromBusiness.AvaialbleBookingsForDateAndRoom.Count());
        }

        [Test]
        public async Task Shoud_Delete_A_Book()
        {
            var fixture = new Fixture();
            var booking = fixture.Build<Booking>()
                .With(b => b.Id, 1)
                .Create();
            var bookingsRepository = Substitute.For<IBookingsRepository>();
            bookingsRepository.DeleteABookingAsync(1).Returns(booking);
            var BookingsBusiness = new BookingsBusiness(bookingsRepository);
            Booking deletedBooking = await BookingsBusiness.DeleteABookingAsync(1);

            Assert.IsNotNull(deletedBooking);
        }
    }
}
