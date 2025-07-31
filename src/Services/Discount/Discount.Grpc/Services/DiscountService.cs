namespace Discount.Grpc.Services;

using Data;
using Entities;
using global::Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;


public class DiscountService(AppDbContext dbContext) : DiscountProtoService.DiscountProtoServiceBase {

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null){
            throw new RpcException(new Status(StatusCode.Internal, "Internal error"));
        }

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .Where(c => c.ProductName == request.ProductName)
            .FirstOrDefaultAsync() ?? new Coupon()
        {
            ProductName = "No product name",
            Amount = 0,
            ProductDescription = "No product description",
        };

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null){
            throw new RpcException(new Status(StatusCode.Internal, "Internal error"));
        }

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .Where(c => c.ProductName == request.ProductName)
            .FirstOrDefaultAsync() ?? new Coupon()
        {
            ProductName = "No product name",
            Amount = 0,
            ProductDescription = "No product description",
        };

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        return new DeleteDiscountResponse() { IsSuccess = true };
    }

}
