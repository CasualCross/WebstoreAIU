using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebstoreAIU.Data;
using WebstoreAIU.Services;

namespace WebstoreAIU.Pages;

public class ImageHandlerModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public ImageHandlerModel(WebstoreDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetMainImageAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item?.MainImage == null || item.MainImage.Length == 0)
        {
            return File(GeneratePlaceholderImage(), "image/svg+xml");
        }

        return File(item.MainImage, "image/jpeg");
    }

    public async Task<IActionResult> OnGetAdditionalImageAsync(int itemId, int imageIndex)
    {
        var item = await _context.Items.FindAsync(itemId);
        if (item == null)
        {
            return NotFound();
        }

        var images = ImageService.ParseAdditionalImages(item.AdditionalImagesJson);
        if (imageIndex < 0 || imageIndex >= images.Count)
        {
            return NotFound();
        }

        var imageData = Convert.FromBase64String(images[imageIndex].Base64Data);
        return File(imageData, images[imageIndex].ContentType);
    }

    private byte[] GeneratePlaceholderImage()
    {
        var svg = "<svg width='400' height='300' xmlns='http://www.w3.org/2000/svg'><rect width='400' height='300' fill='#e0e0e0'/><text x='50%' y='50%' text-anchor='middle' fill='#999' font-family='Arial' font-size='18'>No Image</text></svg>";
        return System.Text.Encoding.UTF8.GetBytes(svg);
    }
}

