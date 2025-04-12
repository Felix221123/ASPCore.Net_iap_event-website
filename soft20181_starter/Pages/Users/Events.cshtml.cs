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
using System.Text.Json;

namespace soft20181_starter.Pages.Users
{
    public class EventsModel : PageModel
    {
        private readonly EventAppDbContext _context;
        public EventsModel(EventAppDbContext context)
        {
            _context = context;
        }
        public List<Event> events { get; set; }


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
                // Get user initials
                UserAbbreviation = GetUserAbbreviation(user.FirstName, user.LastName);

                // Fetch events from the database
                events = _context.Events.OrderByDescending(e => e.CreatedAt).ToList();

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

        public class ImageObj
        {
            public string url { get; set; } = "";
            public string alt { get; set; } = "";
        }



        public JsonResult OnGetEventsJson()
        {
            var dbEvents = _context.Events.ToList();

            // Format into a structure your JS expects (e.g. including nested organizer, venue, date)
            var formatted = dbEvents.Select(e => new
            {
                id = e.EventID,
                name = e.Name,
                description = e.Description,
                type = e.Type,
                date = new
                {
                    day = e.Day,
                    month = e.Month,
                    year = e.Year
                },
                time = e.Time,
                venue = new
                {
                    name = e.VenueName,
                    address = e.VenueAddress
                },
                organizer = new
                {
                    name = e.OrganizerName,
                    contact = e.OrganizerContact
                },
                ticket = new
                {
                    price = e.TicketPrice,
                    currency = e.Currency
                },
                event_link = e.EventLink,
                // ✅ Deserialize the JSON string into a C# object
                images = string.IsNullOrWhiteSpace(e.Images)
                ? new List<ImageObj> { new ImageObj { url = "", alt = e.Name } }
                : JsonSerializer.Deserialize<List<ImageObj>>(e.Images) ?? new List<ImageObj>()

            });

            return new JsonResult(new { events = formatted });
        }

    }
}