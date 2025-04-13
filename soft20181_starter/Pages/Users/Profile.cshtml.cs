using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using soft20181_starter.Models;
using System.Globalization;

namespace soft20181_starter.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public ProfileModel(EventAppDbContext context)
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


        // list of tickets generated
        public List<Ticket> UserTickets { get; set; } = new();

        public IActionResult OnGet()
        {
            var user = GetUserFromSession();
            if (user != null)
            {
                ViewData["User"] = user;
                Console.WriteLine("User is authenticated");
                // Capitalize the first letter of the first name
                UserFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.FirstName.ToUpper());
                UserLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.LastName.ToUpper());
                // Get user initials
                UserAbbreviation = GetUserAbbreviation(user.FirstName, user.LastName);

                UserTickets = _context.Tickets
                .Where(t => t.UserID == user.UserID)
                .Select(t => new Ticket
                {
                    TicketID = t.TicketID,
                    TicketCode = t.TicketCode,
                    IssuedAt = t.IssuedAt,
                    Event = t.Event
                }).ToList();

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
        public string UserLastName {get; set;}

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

        // logout functionality
        public IActionResult OnPostLogout()
        {
            // Remove the session token from the session
            HttpContext.Session.Remove("SessionToken");
            return RedirectToPage("/Users/Login");
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                var attendee = _context.Attendees.FirstOrDefault(a => a.UserID == ticket.UserID && a.EventID == ticket.EventID);
                if (attendee != null)
                    _context.Attendees.Remove(attendee);

                var eventToUpdate = await _context.Events.FindAsync(ticket.EventID);
                if (eventToUpdate != null)
                    eventToUpdate.AttendeeCount--;

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }


    

        

    }
}