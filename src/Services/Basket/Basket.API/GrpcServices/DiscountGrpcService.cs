using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices {
  public class DiscountGrpcService {
    private readonly DiscountService.DiscountServiceClient discountServiceClient;

    public DiscountGrpcService(DiscountService.DiscountServiceClient discountServiceClient) {
      this.discountServiceClient = discountServiceClient;
    }
    
    public async Task<CouponModel> GetDiscountAsync(string productName) {
      var discountRequest = new GetDiscountRequest {ProductName = productName};
      return await discountServiceClient.GetDiscountAsync(discountRequest);
    }
  }
}
