using API.DTOs;
using API.Errors;
using API.Helpers;
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
  public class ProductsController : BaseApiController
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
    public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts(
      [FromQuery] ProductSpecParams productParams)
    {
      var spec = new ProductsWithTypesAndBrandsSpecifications(productParams);

      var countSpec = new ProductWithFiltersWithCountSpecification(productParams);

      var totalItems = await _productsRepo.CountAsync(countSpec);

      var products = await _productsRepo.ListAsync(spec);

      var data =  _mapper
                  .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

      return Ok(new Pagination<ProductToReturnDTO>(productParams.pageIndex, productParams.PageSize, totalItems, data));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecifications(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);

      if(product == null) return NotFound(new ApiResponse(404));

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