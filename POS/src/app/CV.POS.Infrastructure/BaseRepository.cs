using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using CV.POS.Business.Interfaces;
using CV.POS.Data;

namespace CV.POS.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected PosDbContext Db { get; set; }
        protected DbSet<T> DbSet;

        public BaseRepository(PosDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            Db = dbContext;
            DbSet = dbContext.Set<T>();
        }

        #region Select members
        
        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        #endregion

        #region Data Modification

        public void Insert(T entity)
        {
            DbEntityEntry dbEntityEntry = Db.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = Db.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = Db.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        #endregion
    }
}