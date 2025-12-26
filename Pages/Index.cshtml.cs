using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebstoreAIU.Data;
using WebstoreAIU.Models;

namespace WebstoreAIU.Pages;

public class IndexModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public IndexModel(WebstoreDbContext context)
    {
        _context = context;
    }

    public List<Item> Items { get; set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _context.Items.ToListAsync();
    }

    [Authorize]
    public async Task<IActionResult> OnPostAddToCartAsync(int itemId)
    {
        if (!User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Login");
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity++;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                UserId = userId,
                ItemId = itemId,
                Quantity = 1
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}

