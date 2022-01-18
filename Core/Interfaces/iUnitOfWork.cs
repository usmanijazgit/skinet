using Core.Entities;
namespace Core.Interfaces
{
    public interface iUnitOfWork : IDisposable
    {
        iGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
   
}