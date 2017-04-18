using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Domain.Core.Context;

namespace TestOrdersWebProject.Domain.Core
{
    public class Purchase
    {
        public int Id { get; set; }
        public int? CuponId { get; set; }
        public Cupon Cupon { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        [Required]
        [Display(Name = "Summary price")]
        public decimal SummaryPrice { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
