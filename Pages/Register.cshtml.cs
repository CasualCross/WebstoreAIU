using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebstoreAIU.Data;
using WebstoreAIU.Models;
using WebstoreAIU.Services;
using System.ComponentModel.DataAnnotations;

namespace WebstoreAIU.Pages;

public class RegisterModel : PageModel
{
    private readonly WebstoreDbContext _context;

    public RegisterModel(WebstoreDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
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

        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Login == Input.Login))
        {
            ModelState.AddModelError(string.Empty, "A user with this login already exists.");
            return Page();
        }

        // Create new user
        var user = new User
        {
            Login = Input.Login,
            PasswordHash = PasswordHasher.HashPassword(Input.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Login");
    }
}

