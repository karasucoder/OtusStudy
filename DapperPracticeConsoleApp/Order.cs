namespace DapperPracticeConsoleApp
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // навигационные свойства
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
