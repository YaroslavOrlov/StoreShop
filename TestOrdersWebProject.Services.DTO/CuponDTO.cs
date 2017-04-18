using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Services.DTO
{
    public class CuponDTO
    {
        public int Id { get; set; }
        public double Discount { get; set; }

        public int ProductCategoryId { get; set; }
    }
}
