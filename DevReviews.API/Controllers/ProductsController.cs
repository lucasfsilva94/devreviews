using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DevReviews.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [SwaggerTag("Representa os endpoints de produtos")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }


    /// <summary>
    /// Obter todos os produtos 
    /// </summary>
    /// <remarks>
    /// Obtém uma lista de todos os produtos
    /// </remarks>
    /// <returns>retorna todos os produtos cadastrado</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var products = await _repository.GetAllAsync();
      var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);

      return Ok(productsViewModel);
    }

    /// <summary>
    /// Obter produto pelo id
    /// </summary>
    /// <remarks>
    /// Obtém um produto informando o ID do produto
    /// </remarks>
    /// <param name="id">id do produto</param>
    /// <returns>retorna os detalhes do produto</returns>
    /// <response code="200">Sucesso e retorna o produto buscado</response>
    /// <response code="400">Dados Inválidos</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int id)
    {
      var product = await _repository.GetDetailsByIdAsync(id);

      if (product == null) return NotFound("Product does not exist");

      var productDetails = _mapper.Map<ProductDetailsViewModel>(product);

      return Ok(productDetails);
    }

    /// <summary>
    /// Cadastro de Produto
    /// </summary>    
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /Products
    ///     {
    ///        "title": "Um chinelo top",
    ///        "description": "Um chinelo de marca",
    ///        "price": 100 
    ///     }    
    ///
    /// </remarks>    
    /// <param name="model">Objeto com dados de cadastro de Produto</param>    
    /// <returns>Objeto recém-criado</returns>
    /// <response code="201">Sucesso e retorna o produto cadastrado</response>
    /// <response code="400">Dados Inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(AddProductInputModel model)
    {
      if (model.Description.Length >= 50) return BadRequest("Description length must be less than or equal to 50 characters");

      var product = new Product(model.Title, model.Description, model.Price);

      await _repository.AddAsync(product);

      return CreatedAtAction(nameof(GetById), new { id = product.Id }, model);
    }

    /// <summary>
    /// Atualiza dados de um produto
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /Products/1
    ///     {    
    ///        "description": "Um tênis popular",
    ///        "price": 250 
    ///     }    
    ///
    /// </remarks>    
    /// <param name="id">id do produto</param>
    /// <param name="model">objeto com dados do produto a ser atualizado</param>
    /// <returns></returns>
    /// <response code="204">Sucesso sem retorno</response>
    /// <response code="400">Dados Inválidos</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(int id, UpdateProductInputModel model)
    {
      if (model.Description.Length >= 50) return BadRequest("Description length must be less than or equal to 50 characters");

      var product = await _repository.GetByIdAsync(id);

      if (product == null) return NotFound("Product does not exist");

      product.Update(model.Description, model.Price);
      await _repository.UpdateAsync(product);

      return NoContent();
    }
  }
}