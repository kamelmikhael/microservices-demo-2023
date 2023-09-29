using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<DiscountGrpcService> _logger;

    public DiscountGrpcService(
        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
        ILogger<DiscountGrpcService> logger)
    {
        _discountProtoServiceClient = discountProtoServiceClient 
            ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CouponModel> GetDiscountAsync(string productName)
    {
        return await _discountProtoServiceClient.GetDiscountAsync(new() { ProductName = productName });
    }
}
