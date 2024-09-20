using Stripe;
using Stripe.Climate;
using SubscriptionSystem.Model;

namespace SubscriptionSystem.Service.StripeService
{
    public class StripeService : IStripeService
    {
        public StripeService()
        {
            StripeConfiguration.ApiKey = "StripeConfiguration.ApiKeycfsdfasdfgafsdgsdfg   ";
        }
        public async Task<string> CreateCustomerAsync(string email, string paymentMethodId)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                PaymentMethod = paymentMethodId,
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId
                }
            };
            var service = new CustomerService();
            var customer = await service.CreateAsync(options);
            return customer.Id;
        }

        public async Task<string> CreateSubscriptionAsync(string customerId, string priceId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions { Price = priceId }
                },
                Expand = new List<string> { "latest_invoice.payment_intent" }
            };
            var service = new SubscriptionService();
            var subscription = await service.CreateAsync(options);
            return subscription.Id;
        }

        public async Task CancelSubscriptionAsync(string subscriptionId)
        {
            var service = new SubscriptionService();
            await service.CancelAsync(subscriptionId);
        }

        public async Task<StripeProductDto> CreateProductAsync(string name, long amount, string currency, string interval)
        {
            var productOptions = new ProductCreateOptions
            {
                Name = name,
            };
            var productService = new Stripe.ProductService();
            var product = await productService.CreateAsync(productOptions);

            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = amount,
                Currency = currency,
                Recurring = new PriceRecurringOptions { Interval = interval },
                Product = product.Id,
            };
            var priceService = new PriceService();
            var price = await priceService.CreateAsync(priceOptions);

            return new StripeProductDto
            {
                Id = price.Id,
                Name = product.Name,
                Amount = price.UnitAmount.Value,
                Currency = price.Currency,
                Interval = price.Recurring.Interval
            };
        }

        public async  Task<string> CreatePaymentIntent(CreateCustomerPaymnetDto dto)
        {
            var service = new PaymentIntentService();

            PaymentIntent intent = null;
            try
            {
                /*var priceService = new PriceService();
                var prices = await priceService.ListAsync(new PriceListOptions
                {
                    Product = dto.ProductId,
                    Active = true,  
                });*/

                //var price = prices.Data.FirstOrDefault();
                

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)10000,
                    Currency = "inr",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    }
                };
                intent = await service.CreateAsync(options);
                return intent.Id;
            }
            catch (Exception ex)
            {
                return intent.Id;
            }
        }
    }
}
