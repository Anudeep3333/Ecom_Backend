namespace Ecommerce.Models;

public class CartDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}