using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class CartService
{
    
    private readonly EcommerceDbContext _dbContext;

    public CartService(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddToCartAsync(int userId, int productId, int quantity)
    {
        var existingCartItem = await _dbContext.Cart
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            var cartItem = new Cart
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };
            await _dbContext.Cart.AddAsync(cartItem);
        }

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Cart>> GetCartItemsAsync(int userId)
    {
        return await _dbContext.Cart
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public string DeleteCartItem(int id)
    {
        var cartItem = _dbContext.Cart.Find(id);

        if (cartItem == null)
        {
            return "Cart item not found.";
        }

        _dbContext.Cart.Remove(cartItem);
        _dbContext.SaveChanges();

        return "Cart item deleted successfully.";
    }
    
    public string UpdateCartItem(int id, int quantity)
    {
        var existingItem = _dbContext.Cart.Find(id);

        if (existingItem == null)
        {
            return "Cart item not found.";
        }

        existingItem.Quantity = quantity;
        _dbContext.Cart.Update(existingItem);
        _dbContext.SaveChanges();

        return "Cart item updated successfully.";
    }
}