using SupplyManagementSystem.Data;
using SupplyManagementSystem.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SupplyManagementSystem.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity>
    where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public GeneralRepository(ApplicationDbContext context)
        { _context = context; }

        public TEntity Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public ICollection<TEntity> GetAll(string includeProperties = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();

            //return _context.Set<TEntity>().ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties = null, bool tracked = true)
        {
            IQueryable<TEntity> query;
            if (tracked)
            {
                query = _context.Set<TEntity>();

            }
            else
            {
                query = _context.Set<TEntity>().AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }


        public bool Update(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
