namespace Basket.API.Exceptions;

public class BasketNotFoundException : Exception {

    public BasketNotFoundException(string userName) : base($"The basket {userName} was not found.")
    {
    }

}
