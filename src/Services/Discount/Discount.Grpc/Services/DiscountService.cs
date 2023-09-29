using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountReoistory _reoistory;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(
        IDiscountReoistory reoistory,
        ILogger<DiscountService> logger,
        IMapper mapper)
    {
        _reoistory = reoistory ?? throw new ArgumentNullException(nameof(reoistory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _reoistory.GetDiscount(request.ProductName)
            ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName = {request.ProductName} is not found."));

        _logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}",
            coupon.ProductName, coupon.Amount);

        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        var isSuccess = await _reoistory.CreateDiscount(coupon);

        if(!isSuccess)
        {
            throw new RpcException(new Status(StatusCode.Internal,
                $"Discount is NOT created with ProductName = {coupon.ProductName}."));
        }

        _logger.LogInformation($"Discount is successfully created with ProductName = {coupon.ProductName}.");

        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        var isSuccess = await _reoistory.UpdateDiscount(coupon);

        if (!isSuccess)
        {
            throw new RpcException(new Status(StatusCode.Internal,
                $"Discount is NOT updated with ProductName = {coupon.ProductName}."));
        }

        _logger.LogInformation($"Discount is successfully updated with ProductName = {coupon.ProductName}.");

        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var isDeleted = await _reoistory.DeleteDiscount(request.ProductName);

        if (!isDeleted) 
        {
            throw new RpcException(new Status(StatusCode.Internal,
                $"Discount is NOT deleted with ProductName = {request.ProductName}."));
        }

        return new() { Success = isDeleted };
    }
}
