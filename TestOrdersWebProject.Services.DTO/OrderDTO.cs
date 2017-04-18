using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Services.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? CuponId { get; set; }
        public int Count { get; set; }
    }
}
