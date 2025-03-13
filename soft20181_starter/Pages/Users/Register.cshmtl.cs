using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using soft20181_starter.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;



namespace soft20181_starter.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly EventAppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(EventAppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public class RegisterInputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
            public string Password { get; set; }
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

            // ✅ Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }

            // ✅ Create Identity User
            var identityUser = new IdentityUser
            {
                UserName = Input.Email,
                Email = Input.Email
            };

            var result = await _userManager.CreateAsync(identityUser, Input.Password);

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHasher.HashPassword(identityUser, Input.Password);
            identityUser.PasswordHash = hashedPassword;
            
            // ✅ Debug Identity Errors
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Identity Error: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // ✅ Debug if Database Connection Works
            if (!_context.Database.CanConnect())
            {
                Console.WriteLine("❌ ERROR: Cannot connect to the database!");
                return Page();
            }

            // ✅ Create User Entry in SQLite Database
            var user = new User
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                Password = identityUser.PasswordHash, // Store hashed password
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ SUCCESS: User saved in the database!");

            return RedirectToPage("/Users/Login");
        }

    }
}