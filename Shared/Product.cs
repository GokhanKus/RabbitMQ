namespace Shared
{
	public class Product
	{
		public Product(int id, int stock, string name, decimal price)
		{
			Id = id;
			Stock = stock;
			Name = name;
			Price = price;
		}
        public Product()
        {
            
        }
        public int Id { get; set; }
		public int Stock { get; set; }
		public string? Name { get; set; }
		public decimal Price { get; set; }
	}
}
