using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class BillDetailsCreateDto
    {
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
