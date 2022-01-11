using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface iGenericRepository<T> where T : BaseEntity
    {
         Task<T> GetByIdAsync(int id);
         Task<IReadOnlyList<T>> ListAllAsync();
         Task<T> GetEntityWithSpec(iSpecification<T> spec);
         Task<IReadOnlyList<T>> ListAsync(iSpecification<T> spec);

         Task<int> CountAsync(iSpecification<T> spec);
    }
}