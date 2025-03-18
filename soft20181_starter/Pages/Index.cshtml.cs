using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using soft20181_starter.Models;
using System.Globalization;

namespace soft20181_starter.Pages
{

    public class IndexModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public IndexModel(EventAppDbContext context)
        {
            _context = context;
        }

        // This method retrieves the user by session token.
        public User? GetUserFromSession()
        {
            var sessionToken = HttpContext.Session.GetString("SessionToken");
            if (string.IsNullOrEmpty(sessionToken))
                return null; // If there's no session token, return null

            return _context.AppUsers.FirstOrDefault(u => u.SessionToken == sessionToken);
        }

        public IActionResult OnGet()
        {
            var user = GetUserFromSession();
            if (user != null)
            {
                ViewData["User"] = user;
                Console.WriteLine("User is authenticated");
                // Capitalize the first letter of the first name
                UserFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName.ToLower());
                return Page();  // Proceed if user is authenticated
            }
            else
            {
                // Redirect to login if not authenticated
                Console.WriteLine("User is not authenticated");
                return RedirectToPage("/Users/Login");
            }
        }

        public string UserFirstName { get; set; }


    }
}
