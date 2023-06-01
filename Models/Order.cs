namespace Task.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Product Product { get;set; }
        //public User User { get; set; }
        public byte Quantity { get; set; } = 1;
        public int ProductId { get; set; }
        public int UserId { get; set; }

    }
}
