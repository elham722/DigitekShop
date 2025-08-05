using System;

namespace DigitekShop.Domain.BusinessRules
{
    public interface IBusinessRule
    {
        bool IsBroken();
        string Message { get; }
        string RuleName { get; }
        string ErrorCode { get; }
        int Priority { get; }
        bool IsCritical { get; }
    }
} 