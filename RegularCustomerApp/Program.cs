namespace RegularCustomerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var shop = new Shop();

            var customer = new Customer();

            shop.Items.CollectionChanged += customer.OnItemChanged;
        }
    }
}
