using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebstoreAIU.Data;
using WebstoreAIU.Models;
using WebstoreAIU.Services;
using System.Text.Json;

namespace WebstoreAIU.Pages;

[Authorize]
public class AddItemModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public AddItemModel(WebstoreDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

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

        [Required(ErrorMessage = "Main image is required")]
        [Display(Name = "Main Image")]
        public IFormFile? MainImage { get; set; }

        [Display(Name = "Additional Images")]
        public List<IFormFile>? AdditionalImages { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.MainImage == null || Input.MainImage.Length == 0)
        {
            ModelState.AddModelError(nameof(Input.MainImage), "Main image is required");
            return Page();
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var mainImageExtension = Path.GetExtension(Input.MainImage.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(mainImageExtension))
        {
            ModelState.AddModelError(nameof(Input.MainImage), "Invalid image format. Allowed formats: JPG, JPEG, PNG, GIF, WEBP");
            return Page();
        }

        byte[]? mainImageBytes = null;
        using (var memoryStream = new MemoryStream())
        {
            await Input.MainImage.CopyToAsync(memoryStream);
            mainImageBytes = memoryStream.ToArray();
        }

        var additionalImagesList = new List<ItemImage>();
        if (Input.AdditionalImages != null && Input.AdditionalImages.Count > 0)
        {
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
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var item = new Item
        {
            Name = Input.Name,
            Price = Input.Price,
            Description = Input.Description,
            MainImage = mainImageBytes,
            AdditionalImagesJson = ImageService.SerializeAdditionalImages(additionalImagesList),
            OwnerId = userId
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Item", new { id = item.Id });
    }
}

