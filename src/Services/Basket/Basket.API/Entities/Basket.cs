namespace Basket.API.Entities;

using Marten.Schema;


public class Basket {

    [Identity]
    public string UserName { get; set; }

    public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    public Basket(string userName)
    {
        UserName = userName;
    }

    public Basket()
    {
    }

}
