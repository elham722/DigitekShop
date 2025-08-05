namespace DigitekShop.Domain.Enums
{
    public enum PaymentTransactionType
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Refunded = 6,
        Retry = 7,
        GatewayCallback = 8,
        ManualAdjustment = 9
    }
} 