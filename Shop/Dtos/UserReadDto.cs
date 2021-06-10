using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String Role { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
