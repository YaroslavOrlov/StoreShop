using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Data.Repositories;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;

namespace TestOrdersWebProject.Infrastructure.Bussiness
{
    public class MakeProductPurchase : IPurchase
    {
        IDbContext _context;
        GenericRepository<Purchase> _purchases;
        GenericRepository<Product> _products;
        GenericRepository<OrderProduct> _orderProducts;
        GenericRepository<Cupon> _cupons;
        public MakeProductPurchase(IDbContext context)
        {
            _context = context;
            _purchases = new GenericRepository<Purchase>(context);
            _orderProducts = new GenericRepository<OrderProduct>(context);
            _products = new GenericRepository<Product>(context);
            _cupons = new GenericRepository<Cupon>(context);
        }
        public bool MakePurchase(PurchaseDTO purchaseDTO)
        {
            var purchasedProducts = _products.Get(x => purchaseDTO.DiscountedCart.Select(y => y.Id).Contains(x.Id));
            var productsAmount = purchaseDTO.DiscountedCart.GroupBy(x => x.Id).Select(x => new
            {
                ProductId = x.Key,
                Amount = x.Sum(y => y.Count)
            });

            var purchase = new Purchase()
            {
                CuponId = purchaseDTO.DiscountedCart.FirstOrDefault(x => x.CuponId.HasValue)?.CuponId ?? null,
                OrderProducts = productsAmount.Select(x => new OrderProduct()
                {
                    Product = purchasedProducts.Where(y => y.Id == x.ProductId).Single(),
                    Amount = x.Amount
                }).ToList(),
                SummaryPrice = purchaseDTO.SummaryPrice,
                CurrencyId = purchaseDTO.CurrencyId
            };
            
            if (purchase == null)
                return false;

            var cupon = _cupons.Get(x => x.Id == purchase.CuponId).FirstOrDefault();
            if (cupon != null)
            {
                cupon.IsUsed = true;
                _cupons.Update(cupon);
            }
                
            _purchases.Create(purchase);
            _purchases.Save();

            return true;
        }
    }
}
