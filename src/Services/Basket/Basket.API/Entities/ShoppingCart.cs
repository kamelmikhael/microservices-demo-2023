namespace Basket.API.Entities;

public class ShoppingCart
{
    public string Username { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart() { }

    public ShoppingCart(string userName)
    {
        Username = userName;
    }

    public decimal TotalPrice
    {
        get
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Price * item.Quantity;
            }
            return total;
        }
    }
}
