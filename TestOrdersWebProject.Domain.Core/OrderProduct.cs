using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Domain.Core
{
    public class OrderProduct
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Require to select amount of product")]
        public int Amount { get; set; }
    }
}
