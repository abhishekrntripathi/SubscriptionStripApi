namespace SubscriptionSystem.Model
{
    public class StripeProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Interval { get; set; }
    }
}
