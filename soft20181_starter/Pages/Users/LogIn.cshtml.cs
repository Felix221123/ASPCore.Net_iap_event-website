using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using soft20181_starter.Models;
using System.ComponentModel.DataAnnotations;

namespace soft20181_starter.Pages.Users
{
    public class LogInModel : PageModel
    {
        private readonly EventAppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LogInModel(EventAppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }


        public void OnGet()
        {
        }


        // This method retrieves the user by session token.
        public User? GetUserBySessionToken(string sessionToken)
        {
            return _context.AppUsers.FirstOrDefault(u => u.SessionToken == sessionToken);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _context.AppUsers.FirstOrDefault(u => u.Email == Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var verificationResult = passwordHasher.VerifyHashedPassword(new IdentityUser(), user.Password, Input.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var sessionToken = GenerateSessionToken();
            user.SessionToken = sessionToken;

            Console.WriteLine("Session token: " + sessionToken);
            Console.WriteLine("login was successful");

            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();

            // Store the session token in session state (on the server)
            HttpContext.Session.SetString("SessionToken", sessionToken);

            Console.WriteLine("Redirecting to Index page");
            return RedirectToPage("/Index");
        }


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