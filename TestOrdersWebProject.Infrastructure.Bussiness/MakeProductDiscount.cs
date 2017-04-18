using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;

namespace TestOrdersWebProject.Infrastructure.Bussiness
{
    public class MakeProductDiscount : IDiscountProduct
    {
        IGenericRepository<Cupon> _cupons;
        public MakeProductDiscount(IGenericRepository<Cupon> cupons)
        {
            _cupons = cupons;
        }
        public IEnumerable<ProductDTO> ApplyCupon(IEnumerable<ProductDTO> cart, CuponDTO cupon)
        {
            var discountedCart = cart.Where(x => x.CategoriesId != null && x.CategoriesId.Contains(cupon.ProductCategoryId))
                    .Select(x => new ProductDTO()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CategoriesId = x.CategoriesId,
                        Price = x.Price * (decimal)(1 - cupon.Discount / 100),
                        Count = x.Count,
                        Discount = cupon.Discount,
                        CuponId = cupon.Id
                    }).Union(cart.Where(x => x.CategoriesId == null || !x.CategoriesId.Contains(cupon.ProductCategoryId))
                    .Select(x => new ProductDTO()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CategoriesId = x.CategoriesId,
                        Price = x.Price,
                        Count = x.Count,
                        Discount = 0,
                        CuponId = null
                    })).ToList();
            
            return discountedCart;
        }

        public CuponDTO GetCupon(int discountCode)
        {
            var cupon = _cupons.GetWithInclude(x => x.UniqCode == discountCode && !x.IsUsed && DateTime.Now < x.EndTime, x => x.ProductCategory)
                .Select(x => new CuponDTO()
                {
                    Id = x.Id,
                    Discount = x.Discount,
                    ProductCategoryId = x.ProductCategoryId
                }).FirstOrDefault();

            return cupon;    
        }
    }
}
