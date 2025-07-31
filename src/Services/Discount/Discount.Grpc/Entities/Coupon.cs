namespace Discount.Grpc.Entities;

public class Coupon {

    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int Amount { get; set; }

}
