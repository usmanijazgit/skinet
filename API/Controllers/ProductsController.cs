using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
// using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {

    private readonly iGenericRepository<Product> _productsRepo;
    private readonly iGenericRepository<ProductBrand> _productBrandRepo;
    private readonly iGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(iGenericRepository<Product> productsRepo,
    iGenericRepository<ProductBrand> productBrandRepo,
    iGenericRepository<ProductType> productTypeRepo, IMapper mapper)
    {
      _mapper = mapper;
      _productTypeRepo = productTypeRepo;
      _productBrandRepo = productBrandRepo;
      _productsRepo = productsRepo;

    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
    {
      var spec = new ProductsWithTypesAndBrandsSpecifications();

      var products = await _productsRepo.ListAsync(spec);
      return Ok( _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecifications(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);

      return _mapper.Map<Product, ProductToReturnDTO>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
      return Ok(await _productBrandRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
      return Ok(await _productTypeRepo.ListAllAsync());
    }


  }
}