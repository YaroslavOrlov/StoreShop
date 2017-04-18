using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using System.Collections;
using System.Data.Entity.Infrastructure;

namespace TestOrdersWebProject.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> :
            IGenericRepository<TEntity>,
            IReadRepository<TEntity>
            where TEntity : class
    {
        IDbContext _context;
        DbSet<TEntity> _dbSet;

        public GenericRepository(IDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public TEntity FindById(int Id)
        {
            return _dbSet.Find(Id);
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }
        
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Expression<Func<TEntity, bool>> filter, 
            IEnumerable<object> updatedSet, 
            IEnumerable<object> availableSet, 
            string propertyName)
        {
            // Get the generic type of the set
            var type = updatedSet.GetType().GetGenericArguments()[0];

            // Get the previous entity from the database based on repository type
            var previous = _context
                .Set<TEntity>()
                .Include(propertyName)
                .FirstOrDefault(filter);

            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var values = CreateList(type);

            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            foreach (var entry in updatedSet
                .Select(obj => (int)obj.GetType().GetProperty("Id").GetValue(obj, null))
                .Select(value => _context.Set(type).Find(value)))
            {
                values.Add(entry);
            }

            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            _context.Entry(previous).Collection(propertyName).CurrentValue = values;
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }  

        private IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }
    }
}
