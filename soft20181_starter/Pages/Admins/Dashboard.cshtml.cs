using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity; 


namespace soft20181_starter.Pages.Admins
{
    public class DashboardModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public DashboardModel(EventAppDbContext context)
        {
            _context = context;
        }

        public List<Event> AllEvents { get; set; } = new();

        // New property for Contact Messages
        public List<ContactMessage> AllMessages { get; set; } = new();

        [BindProperty]
        public Event Event { get; set; }

        [BindProperty]
        public Event UpdateEvent { get; set; }

        public List<User> AllUsers { get; set; } = new();

        [BindProperty]
        public User SelectedUser { get; set; }


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

            AllEvents = _context.Events.OrderByDescending(e => e.CreatedAt).ToList();
            System.Diagnostics.Debug.WriteLine("Events count: " + AllEvents.Count);

            AllUsers = _context.AppUsers.OrderBy(u => u.CreatedAt).ToList();
            System.Diagnostics.Debug.WriteLine("Users count: " + AllUsers.Count);

            AllMessages = _context.ContactMessages.OrderBy(cm => cm.SentAt).ToList();
            System.Diagnostics.Debug.WriteLine("Messages count: " + AllMessages.Count);

            // Admin is authenticated, proceed to load the dashboard
            return Page();
        }

        // admin logout, by clearing session token
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("AdminSessionToken");
            return RedirectToPage("/Admins/AdminLogIn");
        }


        // save event
        // Handle event addition
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState)
                    {
                        foreach (var subError in error.Value.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Model error on {error.Key}: {subError.ErrorMessage}");
                        }
                    }
                    AllEvents = _context.Events.ToList();
                    AllUsers = _context.AppUsers.ToList();
                    AllMessages = _context.ContactMessages.ToList();
                    TempData["EventSaved"] = false;
                    return Page();
                }

            // Create a new event using the posted data
            var newEvent = new Event
            {
                EventID = Guid.NewGuid(),
                Name = Event.Name,
                Description = Event.Description,
                Type = Event.Type,
                Day = Event.Day,
                Month = Event.Month,
                Year = Event.Year,
                Time = Event.Time,
                VenueName = Event.VenueName,
                VenueAddress = Event.VenueAddress,
                OrganizerName = Event.OrganizerName,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                // Add the new event to the database
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                // Set TempData to show a success message
                TempData["EventSaved"] = true;
                return RedirectToPage(); // Redirect to the same page after saving
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error saving event: {ex.Message}");
                TempData["EventSaved"] = false;
                return Page(); // Return the same page on error
            }
        }



        // update event
        public IActionResult OnPostUpdateEvent()
        {
            System.Diagnostics.Debug.WriteLine("OnPostUpdateEvent() was called");

            if (!ModelState.IsValid)
            {
                AllEvents = _context.Events.OrderByDescending(e => e.CreatedAt).ToList();
                TempData["EventUpdated"] = false;
                return Page();
            }

            var existingEvent = _context.Events.Find(Event.EventID);
            if (existingEvent != null)
            {
                _context.Entry(existingEvent).CurrentValues.SetValues(Event);
                _context.SaveChanges();
                TempData["EventUpdated"] = true;
            }

            return RedirectToPage();
        }



        // delete event
        public IActionResult OnPostDeleteEvent(Guid id)
        {
            var ev = _context.Events.Find(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostUpdateUser()
        {
            var user = _context.AppUsers.Find(SelectedUser.UserID);
            if (user != null)
            {
                user.FirstName = SelectedUser.FirstName;
                user.LastName = SelectedUser.LastName;
                user.Email = SelectedUser.Email;
                user.UpdatedAt = DateTime.UtcNow;

                // If a new password is provided, hash and update it
                if (!string.IsNullOrWhiteSpace(SelectedUser.Password))
                {
                    var identityUser = new IdentityUser { Email = user.Email, UserName = user.Email };
                    var hasher = new PasswordHasher<IdentityUser>();
                    user.Password = hasher.HashPassword(identityUser, SelectedUser.Password);
                }

                _context.SaveChanges();
            }

            return RedirectToPage();
        }


        public IActionResult OnPostDeleteUser(Guid id)
        {
            var user = _context.AppUsers.Find(id);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }


    }
}
