using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Services.DTO;

namespace TestOrdersWebProject.Services.Interfaces
{
    public interface IDiscountProduct
    {
        CuponDTO GetCupon(int discountCode);
        IEnumerable<ProductDTO> ApplyCupon(IEnumerable<ProductDTO> cart, CuponDTO cupon);
    }
}
