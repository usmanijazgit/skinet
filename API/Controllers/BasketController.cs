using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BasketController : BaseApiController
  {
    private readonly iBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    public BasketController(iBasketRepository basketRepository, IMapper mapper)
    {
      _mapper = mapper;
      _basketRepository = basketRepository;
    }


    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
      var basket = await _basketRepository.GetBasketAsyn(id);

      return Ok(basket ?? new CustomerBasket(id));
    }



    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
      var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
      var updatedbasket = await _basketRepository.UpdateBasketAsyn(customerBasket);

      return Ok(updatedbasket);
    }

    [HttpDelete]
    public async Task DeleteBasketAsync(string id)
    {
      await _basketRepository.DeleteBasketAsyn(id);
    }

  }
}