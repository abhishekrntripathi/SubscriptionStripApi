using SubscriptionSystem.Model;

namespace SubscriptionSystem.Service.StripeService
{
    public interface IStripeService
    {
        Task<string> CreateCustomerAsync(string email, string paymentMethodId);
        Task<string> CreateSubscriptionAsync(string customerId, string priceId);
        Task CancelSubscriptionAsync(string subscriptionId);
        Task<StripeProductDto> CreateProductAsync(string name, long amount, string currency, string interval);
        Task<string> CreatePaymentIntent(CreateCustomerPaymnetDto dto);
    }
}
