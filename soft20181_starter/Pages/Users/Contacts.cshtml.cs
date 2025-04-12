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
using System.Globalization;


namespace soft20181_starter.Pages.Users {
    public class ContactsModel : PageModel
    {
        private readonly EventAppDbContext _context;
        public ContactsModel(EventAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContactMessage Contact { get; set; }

        public User? GetUserFromSession()
        {
            var sessionToken = HttpContext.Session.GetString("SessionToken");
            if (string.IsNullOrEmpty(sessionToken))
                return null; // If there's no session token, return null

            return _context.AppUsers.FirstOrDefault(u => u.SessionToken == sessionToken);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.SentAt = DateTime.UtcNow;
            _context.ContactMessages.Add(Contact);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message sent successfully!";
            Console.WriteLine("Model valid: " + ModelState.IsValid);
            Console.WriteLine("Name: " + Contact.FullName);
            Console.WriteLine("Phone: " + Contact.PhoneNumber);
            return RedirectToPage("/Users/Contacts");
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
                // Get user initials
                UserAbbreviation = GetUserAbbreviation(user.FirstName, user.LastName);

                return Page();  // Proceed if user is authenticated
            }
            else
            {
                // Redirect to login if not authenticated
                Console.WriteLine("User is not authenticated");
                return RedirectToPage("/Users/Login");
            }
        }

        // Property to store user's first name
        public string UserFirstName { get; set; }

        // Property to store user's abbreviation (first letter of first and last name)
        public string UserAbbreviation { get; set; }

        // Method to get user's initials
        public string GetUserAbbreviation(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return string.Empty;
            }

            // Get first letter of first and last name
            string abbreviation = firstName.Substring(0, 1).ToUpper() + lastName.Substring(0, 1).ToUpper();
            return abbreviation;
        }
        
    }
}