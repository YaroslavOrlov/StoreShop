using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Services.DTO
{
    public class PurchaseDTO
    {
        public int CurrencyId { get; set; }
        public decimal SummaryPrice { get; set; }

        public List<OrderDTO> DiscountedCart { get; set; }

    }
}
