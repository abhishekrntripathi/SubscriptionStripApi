using Microsoft.AspNetCore.Mvc;
using Stripe;
using SubscriptionSystem.Model;
using SubscriptionSystem.Service.StripeService;

namespace SubscriptionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] StripePaymentRequestDto paymentRequest)
        {
            var customerId = await _stripeService.CreateCustomerAsync(paymentRequest.Email, paymentRequest.PaymentMethodId);
            return Ok(new { CustomerId = customerId });
        }

        [HttpPost("create-subscriptionId")]
        public async Task<IActionResult> CreateSubscriptionId([FromBody] SubscriptionDto subscriptionDto)
        {
            var subscriptionId = await _stripeService.CreateSubscriptionAsync(subscriptionDto.CustomerId, subscriptionDto.ProductId);
            return Ok(new { SubscriptionId = subscriptionId });
        }

        [HttpPost("cancel-subscription")]
        public async Task<IActionResult> CancelSubscription([FromBody] SubscriptionDto subscriptionDto)
        {
            await _stripeService.CancelSubscriptionAsync(subscriptionDto.SubscriptionId);
            return NoContent();
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] StripeProductDto productDto)
        {
            var product = await _stripeService.CreateProductAsync(productDto.Name, productDto.Amount, productDto.Currency, productDto.Interval);
            return Ok(product);
        }

        [HttpPost("create-payment-methodId")]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] TokenRequest tokenRequest)
        {
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Token = tokenRequest.Token
                }
            };

            var service = new PaymentMethodService();
            try
            {
                var paymentMethod = await service.CreateAsync(options);
                return Ok(new { PaymentMethodId = paymentMethod.Id });
            }
            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.StripeError.Message });
            }
        }


       
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdatePaymentIntent(CreateCustomerPaymnetDto dto)
        {

            var response = await _stripeService.CreatePaymentIntent(dto);
            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], "whsec_5D2BxHv2eDPtExNUm14pluEAW2tYb3jG");

                
                if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    // code
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    // code
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
