using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Data.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;

namespace TestOrdersWebProject.UnitOfWorks
{
    public class PurchasesUnitOfWork
    {

        IDbContext _context;
        IPurchase _makePurchase;
        GenericRepository<Purchase> _purhases;
        public PurchasesUnitOfWork(IDbContext context, IPurchase makePurchase)
        {
            _context = context;
            _makePurchase = makePurchase;
        }

        public GenericRepository<Purchase> Purchases
        {
            get
            {
                if (_purhases == null)
                    _purhases = new GenericRepository<Purchase>(_context);
                return _purhases;
            }
        }

        public bool MakePurchase(PurchaseDTO purchase)
        {
            var isSuccess = _makePurchase.MakePurchase(purchase);

            return isSuccess;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}