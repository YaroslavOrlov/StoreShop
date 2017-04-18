using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Services.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public int? CuponId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
        public int[] CategoriesId { get; set; }
        public int Count { get; set; }
    }
}
