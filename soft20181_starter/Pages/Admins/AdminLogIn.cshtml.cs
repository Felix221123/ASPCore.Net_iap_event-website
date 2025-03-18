using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;


namespace soft20181_starter.Pages.Admins
{
    public class AdminLogInModel : PageModel
    {
        private readonly EventAppDbContext _context;
        public AdminLogInModel(EventAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdminLoginInputModel Input { get; set; }

        public class AdminLoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }  // Admin Password

            [Required]
            public string AdminKey { get; set; }  // AdminKey for additional verification
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

            // Step 1: Check if the email exists in the database
            var admin = _context.Admins.FirstOrDefault(a => a.Email == Input.Email);
            if (admin == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or admin key.");
                return Page();
            }

            // Step 2: Verify Admin Password
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(new IdentityUser(), admin.Password, Input.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid password.");
                return Page();
            }

            // Step 3: Verify AdminKey
            var adminKeyVerificationResult = passwordHasher.VerifyHashedPassword(new IdentityUser(), admin.AdminKey, Input.AdminKey);
            if (adminKeyVerificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid admin key.");
                return Page();
            }

            // Step 4: If both password and admin key are verified, generate a session token
            var sessionToken = GenerateSessionToken();
            admin.SessionToken = sessionToken;

            // Save the session token in the database
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            // Store the session token in session state (on the server)
            HttpContext.Session.SetString("AdminSessionToken", sessionToken);

            // Redirect to the admin dashboard after successful login
            return RedirectToPage("/Admins/Dashboard");
        }

        // Helper method to generate a session token
        private string GenerateSessionToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var tokenBytes = new byte[32];
                rng.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }
    }
}