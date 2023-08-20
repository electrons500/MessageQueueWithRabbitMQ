namespace RabbitmqConsumerDemo.Model
{
    public class OrderRequestModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
    }
}
