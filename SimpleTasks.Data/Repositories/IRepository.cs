using System.Collections.Generic;

namespace SimpleTasks.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> List();
        TEntity GetById(int id);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
