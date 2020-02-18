using RoomBooking.Common.Models;
using System.Threading.Tasks;

namespace RoomBooking.Business.Interfaces
{
    public interface IBookingsBusiness
    {
        Task<CreatedBooking> BookARoomAsync(Booking booking);
        Task<Booking> DeleteABookingAsync(int id);
    }
}
