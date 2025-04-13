using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using System.Linq;

namespace soft20181_starter.Pages.Admins
{
    public class DashboardModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public DashboardModel(EventAppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var sessionToken = HttpContext.Session.GetString("AdminSessionToken");

            if (string.IsNullOrEmpty(sessionToken))
            {
                // No session token, redirect to login
                return RedirectToPage("/Admins/AdminLogIn");
            }

            var admin = _context.Admins.FirstOrDefault(a => a.SessionToken == sessionToken);

            if (admin == null)
            {
                // Invalid session token, redirect to login
                return RedirectToPage("/Admins/AdminLogIn");
            }

            // Admin is authenticated, proceed to load the dashboard
            return Page();
        }

        // admin logout, by clearing session token
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("AdminSessionToken");
            return RedirectToPage("/Admins/AdminLogIn");
        }

    }
}
