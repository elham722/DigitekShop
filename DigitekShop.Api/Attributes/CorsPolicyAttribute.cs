using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitekShop.Api.Attributes
{
    /// <summary>
    /// Custom CORS policy attribute that can be applied to controllers or actions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CorsPolicyAttribute : EnableCorsAttribute
    {
        public CorsPolicyAttribute(string policyName = "DevelopmentPolicy") : base(policyName)
        {
        }
    }

    /// <summary>
    /// Attribute for allowing all CORS origins (use with caution)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AllowAllCorsAttribute : EnableCorsAttribute
    {
        public AllowAllCorsAttribute() : base("AllowAll")
        {
        }
    }

    /// <summary>
    /// Attribute for production CORS policy
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ProductionCorsAttribute : EnableCorsAttribute
    {
        public ProductionCorsAttribute() : base("ProductionPolicy")
        {
        }
    }
} 