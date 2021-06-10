using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Shop.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly ShopContext _context;
        public UsersServices(ShopContext context)
        {
            _context = context;

        }
        public void AddUser(User user)
        {
          if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);

        }

        public void DeleteUser(User user)
        {
            if(user== null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Remove(user) ;
            
        }

        public User GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList<User>();
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges()>=0);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }


    }
}
