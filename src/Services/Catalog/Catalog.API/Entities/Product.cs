namespace Catalog.API.Entities;

public class Product {

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string ImageUrl { get; set; }

    public List<string> Categories { get; set; } = new List<string>();

}
