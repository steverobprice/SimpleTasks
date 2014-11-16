using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SimpleTasks.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TasksDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(TasksDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> List()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Insert(TEntity entity)
        {
            var newEntity = _dbSet.Add(entity);
            _context.SaveChanges();
            return newEntity;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }
    }
}
