using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
  public class BasketRepository : iBasketRepository
  {
    private readonly IDatabase _database;
    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
      return await _database.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket> GetBasketAsyn(string basketId)
    {
      var data = await _database.StringGetAsync(basketId);

      return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
    }

    public async Task<CustomerBasket> UpdateBasketAsyn(CustomerBasket basket)
    {
      var created = await _database.StringSetAsync(basket.Id, 
          JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

          if(!created) return null;

        return await GetBasketAsyn(basket.Id);
    }
  }
}