﻿using Discount.API.Entities;

namespace Discount.API.Repositories;

public interface IDiscountReoistory
{
    Task<Coupon> GetDiscount(string productName);

    Task<bool> CreateDiscount(Coupon coupon);

    Task<bool> UpdateDiscount(Coupon coupon);

    Task<bool> DeleteDiscount(string productName);
}