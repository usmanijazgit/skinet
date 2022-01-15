using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BasketController : BaseApiController
  {
    private readonly iBasketRepository _basketRepository;
    public BasketController(iBasketRepository basketRepository)
    {
      _basketRepository = basketRepository;
    }


    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
        var basket = await _basketRepository.GetBasketAsyn(id);

        return Ok(basket ?? new CustomerBasket(id));
    }


    
    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
    {
        var updatedbasket = await _basketRepository.UpdateBasketAsyn(basket);

        return Ok(updatedbasket);
    }

    [HttpDelete]
    public async Task DeleteBasketAsync(string id)
    {
        await _basketRepository.DeleteBasketAsyn(id);
    }

  }
}