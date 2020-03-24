namespace Portal.Shared.Model.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderMnemonic { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string BtcPrice { get; set; }
        public string Rate { get; set; }
    }
}