using System;

namespace DigitekShop.Domain.BusinessRules
{
    public abstract class BaseBusinessRule : IBusinessRule
    {
        public abstract bool IsBroken();
        public abstract string Message { get; }
        public abstract string RuleName { get; }
        public abstract string ErrorCode { get; }
        public virtual int Priority => 0;
        public virtual bool IsCritical => false;

        protected BaseBusinessRule()
        {
        }
    }
} 