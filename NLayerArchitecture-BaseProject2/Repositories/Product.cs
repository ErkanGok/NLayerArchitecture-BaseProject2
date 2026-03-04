namespace APP.Repositories;

public class Product
{
	public int ID { get; set; }
	public string Name { get; set; } = default!;
	public decimal Price { get; set; }
	public int Stock { get; set; }
}

