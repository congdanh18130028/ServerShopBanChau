using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class BillCreateDto
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public int State { get; set; }
        public Boolean IsPay { get; set; }
    }
}
