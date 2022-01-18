using Core.Entities;

namespace Core.Interfaces
{
    public interface iBasketRepository
    {
         Task<CustomerBasket> GetBasketAsyn(string basketId);
         Task<CustomerBasket> UpdateBasketAsyn(CustomerBasket basket);
         Task<bool> DeleteBasketAsync(string basketId);
    }
}