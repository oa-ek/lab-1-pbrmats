using PBRmats.Repositories.Interfaces;
using PBRmats.Core.Entities;
using PBRmats.Core.Context;

namespace PBRmats.Repositories.Repos
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
         where TEntity : Entity
    {
        private PBRmatsContext _context;

        public Repository(PBRmatsContext context)
        {
            _context = context;
        }

        public void Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            Save();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            Save();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            Save();
        }

        public TEntity Get(TKey key) => 
            _context.Set<TEntity>().Find(key);

        public IEnumerable<TEntity> GetAll() =>
            _context.Set<TEntity>().ToList();

        public void Save() => 
            _context.SaveChanges();
    }
}
