using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebstoreAIU.Data;

namespace WebstoreAIU.Pages;

[Authorize]
public class CartModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public CartModel(WebstoreDbContext context)
    {
        _context = context;
    }

    public List<Models.CartItem> CartItems { get; set; } = new();
    public decimal Total { get; set; }

    public async Task OnGetAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        CartItems = await _context.CartItems
            .Include(c => c.Item)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        Total = CartItems.Sum(c => c.Item.Price * c.Quantity);
    }

    public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Cart");
    }
}

