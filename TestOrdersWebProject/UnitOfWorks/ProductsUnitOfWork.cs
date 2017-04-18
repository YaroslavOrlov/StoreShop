using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Data.Repositories;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;

namespace TestOrdersWebProject.UnitOfWorks
{
    public class ProductsUnitOfWork
    {
        IDbContext _context;
        GenericRepository<Product> _products;
        GenericRepository<ProductCharacteristic> _productsCharacteristics;
        GenericRepository<ProductCategory> _productCategories;
        public ProductsUnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Product> Products
        {
            get
            {
                if(_products == null)
                    _products = new GenericRepository<Product>(_context);
                return _products;
            }
        }
        public GenericRepository<ProductCharacteristic> ProductsCharacteristics
        {
            get
            {
                if (_productsCharacteristics == null)
                    _productsCharacteristics = new GenericRepository<ProductCharacteristic>(_context);
                return _productsCharacteristics;
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

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}