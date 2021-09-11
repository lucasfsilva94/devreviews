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

namespace DevReviews.API.Controllers
{
  [ApiController]
  [Route("api/Products/{productId}/ProductReviews")]
  public class ProductReviewsController : ControllerBase
  {
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    public ProductReviewsController(IProductRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    /// <summary>
    /// Obtêm a review do produto pelo id
    /// </summary>
    /// <param name="productId">id do produto</param>
    /// <param name="id">id da review do produto</param>
    /// <returns>retorna os dados da review do produto</returns>    
    /// <response code="200">Sucesso e retorna a review do produto buscado</response>
    /// <response code="400">Dados Inválidos</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int productId, int id)
    {
      var productReview = await _repository.GetReviewByIdAsync(id);

      if (productReview == null) return NotFound("Product review does not exist");

      var productDetails = _mapper.Map<ProductReviewDetailsViewModel>(productReview);

      return Ok(productDetails);
    }

    /// <summary>
    /// Cadastro da Review do Produto
    /// </summary>    
    /// <remarks>
    /// Requisição:
    ///
    ///     POST /​Products​/1​/ProductReviews
    ///     {
    ///        "rating": 8,
    ///        "author": "Lucas",
    ///        "comments": "produto muito confortavel" 
    ///     }    
    ///
    /// </remarks>    
    /// <param name="productId">Id do produto</param>    
    /// <param name="model">Objeto com dados de cadastro da Review do produto</param>    
    /// <returns>Objeto recém-criado</returns>
    /// <response code="201">Sucesso e retorna a review cadastrada</response>
    /// <response code="400">Dados Inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(int productId, AddProductReviewInputModel model)
    {
      var productReview = new ProductReview(model.Author, model.Rating, model.Comments, productId);

      await _repository.AddReviewAsync(productReview);

      return CreatedAtAction(nameof(GetById), new { id = productReview.Id, productId = productId }, productReview);
    }
  }
}