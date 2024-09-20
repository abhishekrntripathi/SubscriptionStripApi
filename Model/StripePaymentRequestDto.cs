namespace SubscriptionSystem.Model
{
    public class StripePaymentRequestDto
    {
        public string Email { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
