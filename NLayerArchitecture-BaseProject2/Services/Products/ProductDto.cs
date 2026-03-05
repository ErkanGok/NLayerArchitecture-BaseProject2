using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
	public record ProductDto(int Id, string Name, decimal Price, int Stock);
	//public record ProductDto
	//{
	//	public int ID { get; init; }
	//	public string Name { get; init; };
	//	public decimal Price { get; init; }
	//	public int Stock { get; init; }
	//}
}
