using Microsoft.AspNetCore.JsonPatch;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services

{
    public interface IUsersServices
    {
        public List<User> GetUsers();
        public User GetUser(int id);
        public void AddUser(User user);
        public void DeleteUser(User user);
        public bool SaveChanges();
        public void UpdateUser(User user);
        public User GetUserByEmail(String email);
    }
}
