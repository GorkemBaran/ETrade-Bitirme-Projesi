using AppCore.Records.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppCore.DataAccess.EntityFramework.Bases
{
    public abstract class RepoBase<TEntity> : IDisposable where TEntity : RecordBase, new() // Repository Pattern
    {
        public DbContext DbContext { get; set; }

        protected RepoBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            var query = Query(entitiesToInclude);
            query = query.Where(predicate);
            return query;
        }

        public virtual IQueryable<TRelationalEntity> Query<TRelationalEntity>() where TRelationalEntity : class, new()
        {
            return DbContext.Set<TRelationalEntity>().AsQueryable();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] entitiesToInclude)
        {
            return Query(entitiesToInclude).Any(predicate);
        }

        public virtual void Add(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString();
            DbContext.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        public virtual void Update(TEntity entity, bool save = true)
        {
            DbContext.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        public virtual void Delete(TEntity entity, bool save = true)
        {
            DbContext.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }

        public virtual void Delete(int id, bool save = true)
        {
            //var entity = DbContext.Set<TEntity>().Find(id);

            //var entity = Query().ToList();
            //var entity = Query().Single(e => e.Id == id);
            //var entity = Query().First(e => e.Id == id);
            //var entity = Query().Last(e => e.Id == id);
            //var entities = Query().Where(e => e.Id == id).ToList();
            //var entity = Query().Where(e => e.Id == id).SingleOrDefault();

            var entity = Query().SingleOrDefault(e => e.Id == id);
            Delete(entity, save);

            //var entity = Query().FirstOrDefault(e => e.Id == id);
            //var entity = Query().LastOrDefault(e => e.Id == id);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate, bool save = true)
        {
            var entities = Query(predicate).ToList();
            foreach (var entity in entities)
            {
                Delete(entity, false);
            }
            if (save)
                Save();
        }

        public virtual int Save()
        {
            try
            {
                return DbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                // hata loglama kodları yazılabilir
                throw exc;
            }
        }

        public void Dispose()
        {
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
