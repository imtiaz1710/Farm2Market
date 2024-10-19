using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No Discount found"};

        logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount Successfully Created for ProductName: {productName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount Successfully Updated for ProductName: {productName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if(coupon == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

        dbContext.Coupons.Remove(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount Successfully Deleted for ProductName: {productName}", coupon.ProductName);

        return new DeleteDiscountResponse() { Success = true };
    }
}
