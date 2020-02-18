using RoomBooking.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Business.Interfaces
{
    public interface IRoomsBusiness
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
    }
}
