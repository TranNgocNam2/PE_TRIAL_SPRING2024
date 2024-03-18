using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        TEntity? GetById(object? id);

        IEnumerable<TEntity> Get(
            int? pageIndex = null,
            int? pageSize = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
        
       IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, int? pageIndex, int? pageSize, params Expression<Func<TEntity, object>>[]? includeProperties);
       IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[]? includeProperties);

    }
}
