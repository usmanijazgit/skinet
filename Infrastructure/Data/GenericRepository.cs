using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class GenericRepository<T> : iGenericRepository<T> where T : BaseEntity
  {
    private readonly StoreContext _context;
    public GenericRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<T> GetByIdAsync(int id)
    {
      return await _context.Set<T>().FindAsync(id);
    }

  
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
      return await _context.Set<T>().ToListAsync();
    }

      public async Task<T> GetEntityWithSpec(iSpecification<T> spec)
    {
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }


    public async Task<IReadOnlyList<T>> ListAsync(iSpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }


    public async Task<int> CountAsync(iSpecification<T> spec)
    {
     return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(iSpecification<T> spec)
    {
      return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }

   
  }
}