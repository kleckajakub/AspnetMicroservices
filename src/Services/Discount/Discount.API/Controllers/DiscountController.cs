using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers {
  [ApiController]
  [Route("api/v1/[controller]")]
  public class DiscountController : ControllerBase {
    private readonly IDiscountRepository repository;

    public DiscountController(IDiscountRepository repository) {
      this.repository = repository;
    }
    
    [HttpGet("{productName}", Name = "GetDiscount")]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName) {
      var coupon = await repository.GetDiscountAsync(productName);

      return Ok(coupon);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon) {
      await repository.CreateDiscountAsync(coupon);

      return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon) {
      var affected = await repository.UpdateDiscountAsync(coupon);

      return Ok(affected);
    }
    
    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteDiscount(string productName) {
      var affected = await repository.DeleteDiscountAsync(productName);

      return Ok(affected);
    }
  }
}
