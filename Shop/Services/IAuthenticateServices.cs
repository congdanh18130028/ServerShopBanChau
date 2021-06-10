using Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface IAuthenticateServices
    {
        public UserReadDto Authenticate(string email, string password);
        public string GenerateJSONWebToken(UserReadDto customer);
    }
}
