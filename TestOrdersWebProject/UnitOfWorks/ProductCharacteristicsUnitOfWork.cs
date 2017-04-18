using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Data.Repositories;

namespace TestOrdersWebProject.UnitOfWorks
{
    public class ProductCharacteristicsUnitOfWork
    {
        IDbContext _context;
        GenericRepository<ProductCharacteristic> _characteristics;
        GenericRepository<CharacteristicsGroup> _characteristicsGroup;
        public ProductCharacteristicsUnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public GenericRepository<ProductCharacteristic> Characteristics
        {
            get
            {
                if (_characteristics == null)
                    _characteristics = new  GenericRepository<ProductCharacteristic>(_context);
                return _characteristics;
            }
        }
        public GenericRepository<CharacteristicsGroup> CharacteristicsGroup
        {
            get
            {
                if (_characteristicsGroup == null)
                    _characteristicsGroup = new GenericRepository<CharacteristicsGroup>(_context);
                return _characteristicsGroup;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}