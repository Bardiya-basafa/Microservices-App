﻿namespace Basket.API.Entities;

public class BasketItem {

    public int Quantity { get; set; }

    public string Color { get; set; }

    public decimal Price { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; }

}
