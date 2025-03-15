using Someren.Models;

namespace Someren.Repositories
{
    public interface IRoomRepository
    {
        List<Room> GetAll();
        Room? GetByRoomNumber(string roomNumber);
        void Add(Room Room);
        void Update(Room Room);
        void Delete(Room Room);
    }
}
