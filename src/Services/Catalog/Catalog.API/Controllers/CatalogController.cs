using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
  [ApiController]
  [Route("api/v1/[controller]")]
  public class CatalogController : ControllerBase {
    private readonly IProductRepository repository;
    private readonly ILogger<CatalogController> logger;

    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger) {
      this.repository = repository;
      this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
      var products = await repository.GetProducts();

      return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProduct(string id) {
      var product = await repository.GetProduct(id);

      if (product == null) {
        logger.LogError($"Product with id: {id}, not found.");

        return NotFound();
      }

      return Ok(product);
    }

    [Route("[action]/{category}", Name = "GetProductsByCategory")]
    [HttpGet]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category) {
      var product = await repository.GetProductsByCategory(category);

      return Ok(product);
    }

    [Route("[action]/{name}", Name = "GetProductsByName")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name) {
      var product = await repository.GetProductsByName(name);

      return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product) {
      await repository.CreateProduct(product);
      
      return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product) {
      return Ok(await repository.UpdateProduct(product));
    }


    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id) {
      return Ok(await repository.DeleteProduct(id));
    }
  }
}
