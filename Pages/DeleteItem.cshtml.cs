using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebstoreAIU.Data;

namespace WebstoreAIU.Pages;

[Authorize]
public class DeleteItemModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public DeleteItemModel(WebstoreDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == userId);

        if (item == null)
        {
            return NotFound();
        }

        // Remove all cart items that reference this item
        var cartItems = await _context.CartItems
            .Where(c => c.ItemId == id)
            .ToListAsync();
        
        _context.CartItems.RemoveRange(cartItems);
        
        // Remove the item
        _context.Items.Remove(item);
        
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}

