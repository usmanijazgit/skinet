using System.Collections.Generic;
using API.Entities;

namespace Core.Interfaces
{
    public interface iProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
    }
}