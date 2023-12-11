using PBRmats.Core.Entities;

namespace PBRmats.Repositories.Interfaces
{
    public interface IListService<TEntity> where TEntity : Entity
    {
        ICollection<TEntity> GetList();
    }
}
