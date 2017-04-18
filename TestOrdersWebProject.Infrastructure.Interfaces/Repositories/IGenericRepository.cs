using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Infrastructure.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> :
          IIncludeRepostory<TEntity>,
          IReadRepository<TEntity>
          where TEntity : class
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Update(Expression<Func<TEntity, bool>> filter,
                    IEnumerable<object> updatedSet, // Updated many-to-many relationships
                    IEnumerable<object> availableSet, // Lookup collection
                    string propertyName);
        void Remove(TEntity entity);
        void Save();
    }
}
