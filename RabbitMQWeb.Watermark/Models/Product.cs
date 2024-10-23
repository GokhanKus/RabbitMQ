using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQWeb.Watermark.Models
{
	public class Product
	{
		public Product(int id, int stock, string name, decimal price, string imageUrl)
		{
			Id = id;
			Stock = stock;
			Name = name;
			Price = price;
			ImageUrl = imageUrl;
		}
		public Product()
		{

		}
		[Key]
		public int Id { get; set; }

		[Range(0,100)]
		public int Stock { get; set; }

		[StringLength(100)]
		public string? Name { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[StringLength(100)]
        public string? ImageUrl{ get; set; }
    }
}
