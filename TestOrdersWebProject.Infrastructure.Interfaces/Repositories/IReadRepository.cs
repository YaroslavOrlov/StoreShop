using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrdersWebProject.Infrastructure.Interfaces.Repositories
{
    public interface IReadRepository<TEntity>
         where TEntity : class
    {
        TEntity FindById(int Id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
    }
}
