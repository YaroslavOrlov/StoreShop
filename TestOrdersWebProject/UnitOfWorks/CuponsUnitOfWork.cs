using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Data.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;

namespace TestOrdersWebProject.UnitOfWorks
{
    public class CuponsUnitOfWork
    {
        IDbContext _context;
        IDiscountProduct _makeDiscount;
        GenericRepository<Cupon> _cupons;
        GenericRepository<Currency> _currencies;
        GenericRepository<ProductCategory> _productCategories;
        public CuponsUnitOfWork(IDbContext context, IDiscountProduct makeDiscount)
        {
            _context = context;
            _makeDiscount = makeDiscount;
        }

        public GenericRepository<Cupon> Cupons
        {
            get
            {
                if (_cupons == null)
                    _cupons = new GenericRepository<Cupon>(_context);
                return _cupons;
            }
        }

        public GenericRepository<Currency> Currencies
        {
            get
            {
                if (_currencies == null)
                    _currencies = new GenericRepository<Currency>(_context);
                return _currencies;
            }
        }
        public GenericRepository<ProductCategory> ProductCategories
        {
            get
            {
                if (_productCategories == null)
                    _productCategories = new GenericRepository<ProductCategory>(_context);
                return _productCategories;
            }
        }

        public IEnumerable<ProductDTO> MakeDiscount(List<ProductDTO> cart, int discountCode)
        {
            var cupon = _makeDiscount.GetCupon(discountCode);

            if (cupon == null)
                return cart;

            var discountedCart = _makeDiscount.ApplyCupon(cart, cupon);

            return discountedCart;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}