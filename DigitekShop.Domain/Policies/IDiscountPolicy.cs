using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Policies
{
    public interface IDiscountPolicy
    {
        bool IsEligible(Product product);
        Money CalculateDiscount(Product product);
        string GetPolicyName();
    }
} 