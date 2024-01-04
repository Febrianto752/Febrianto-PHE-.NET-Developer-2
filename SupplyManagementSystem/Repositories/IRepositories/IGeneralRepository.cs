using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SupplyManagementSystem.Repositories.IRepositories
{
    public interface IGeneralRepository<TEntity>
    {
        ICollection<TEntity> GetAll(string includeProperties = null);
        TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties = null, bool tracked = true);
        TEntity Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);

        void SaveChanges();
    }
}
