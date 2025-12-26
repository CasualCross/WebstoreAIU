using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebstoreAIU.Data;
using WebstoreAIU.Models;
using WebstoreAIU.Services;

namespace WebstoreAIU.Pages;

[Authorize]
public class EditItemModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public EditItemModel(WebstoreDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public Item? Item { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Item name is required")]
        [Display(Name = "Item Name")]
        [StringLength(200, ErrorMessage = "Item name must be less than 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [StringLength(2000, ErrorMessage = "Description must be less than 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Main Image")]
        public IFormFile? MainImage { get; set; }

        [Display(Name = "Additional Images")]
        public List<IFormFile>? AdditionalImages { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        Item = await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == userId);

        if (Item == null)
        {
            return NotFound();
        }

        Input.Name = Item.Name;
        Input.Price = Item.Price;
        Input.Description = Item.Description;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        Item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == userId);

        if (Item == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Validate image file types if new images are provided
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Update main image if provided
        if (Input.MainImage != null && Input.MainImage.Length > 0)
        {
            var mainImageExtension = Path.GetExtension(Input.MainImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(mainImageExtension))
            {
                ModelState.AddModelError(nameof(Input.MainImage), "Invalid image format. Allowed formats: JPG, JPEG, PNG, GIF, WEBP");
                return Page();
            }

            using var memoryStream = new MemoryStream();
            await Input.MainImage.CopyToAsync(memoryStream);
            Item.MainImage = memoryStream.ToArray();
        }

        // Update additional images if provided
        if (Input.AdditionalImages != null && Input.AdditionalImages.Count > 0)
        {
            var additionalImagesList = new List<ItemImage>();
            foreach (var image in Input.AdditionalImages)
            {
                if (image.Length > 0)
                {
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                    if (allowedExtensions.Contains(extension))
                    {
                        using var memoryStream = new MemoryStream();
                        await image.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        
                        var contentType = extension switch
                        {
                            ".jpg" or ".jpeg" => "image/jpeg",
                            ".png" => "image/png",
                            ".gif" => "image/gif",
                            ".webp" => "image/webp",
                            _ => "image/jpeg"
                        };

                        additionalImagesList.Add(new ItemImage
                        {
                            Base64Data = Convert.ToBase64String(imageBytes),
                            ContentType = contentType
                        });
                    }
                }
            }
            
            if (additionalImagesList.Any())
            {
                Item.AdditionalImagesJson = ImageService.SerializeAdditionalImages(additionalImagesList);
            }
        }

        // Update item properties
        Item.Name = Input.Name;
        Item.Price = Input.Price;
        Item.Description = Input.Description;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Item", new { id = Item.Id });
    }
}

