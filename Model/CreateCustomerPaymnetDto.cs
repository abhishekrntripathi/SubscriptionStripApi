namespace SubscriptionSystem.Model
{
    public class CreateCustomerPaymnetDto
    {
        public string CustomerId { get; set; }

        public string SubscriptionId { get; set; }

        public string ProductId { get; set; }

        public string PaymentMethodId { get; set; }
    }
}
