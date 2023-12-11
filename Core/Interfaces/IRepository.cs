using PBRmats.Core.Entities;

namespace PBRmats.Repositories.Interfaces
{
    public interface IRepository<TEntity, TKey>
         where TEntity : Entity
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Save();
        IEnumerable<TEntity> GetAll();
        TEntity Get(TKey key);
    }
}
