using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services {
  public class DiscountService : Protos.DiscountService.DiscountServiceBase {
    private readonly IDiscountRepository discountRepository;
    private readonly IMapper mapper;
    private readonly ILogger<DiscountService> logger;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper, ILogger<DiscountService> logger) {
      this.discountRepository = discountRepository;
      this.mapper = mapper;
      this.logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context) {
      var coupon = await discountRepository.GetDiscountAsync(request.ProductName);

      if (coupon == null) {
        throw new RpcException(new Status(StatusCode.NotFound, "Discount with specified product name not found."));
      }
      
      logger.LogInformation("Discount is retrieved for product name : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

      return mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context) {
      var coupon = mapper.Map<Entities.Coupon>(request.Coupon);

      await discountRepository.CreateDiscountAsync(coupon);
      logger.LogInformation("Discount is successfully created. ProductName : {productName}", coupon.ProductName);

      return mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context) {
      var coupon = mapper.Map<Entities.Coupon>(request.Coupon);
      
      await discountRepository.UpdateDiscountAsync(coupon);
      logger.LogInformation("Discount is successfully updated. ProductName : {productName}", coupon.ProductName);
      
      return mapper.Map<CouponModel>(coupon);
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context) {
      var deleted = await discountRepository.DeleteDiscountAsync(request.ProductName);

      return new DeleteDiscountResponse {Success = deleted};
    }
  }
}
