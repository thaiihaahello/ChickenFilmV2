using ChickenFilmV2.Models;
using System.Collections.Generic;

namespace ChickenFilmV2.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}