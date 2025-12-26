using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebstoreAIU.Data;
using WebstoreAIU.Models;

namespace WebstoreAIU.Pages;

public class ItemModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public ItemModel(WebstoreDbContext context)
    {
        _context = context;
    }

    public Item? Item { get; set; }
    public bool IsOwner { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Item = await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == id);
        
        if (Item == null)
        {
            return NotFound();
        }

        // Check if current user is the owner
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            IsOwner = Item.OwnerId == userId;
        }

        return Page();
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
        return RedirectToPage("/Item", new { id = itemId });
    }
}

