using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages.Users
{
    public class LogoutModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public LogoutModel(EventAppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Remove the session token from the session
            HttpContext.Session.Remove("SessionToken");

            return RedirectToPage("/Users/Login");
        }
    }
}
