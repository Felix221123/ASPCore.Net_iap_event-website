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
            Console.WriteLine("Events count: " + AllEvents.Count);

            AllUsers = _context.AppUsers.OrderBy(u => u.CreatedAt).ToList();
            Console.WriteLine("Users count: " + AllUsers.Count);

            AllMessages = _context.ContactMessages.OrderBy(cm => cm.SentAt).ToList();
            Console.WriteLine("Messages count: " + AllMessages.Count);

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
            Console.WriteLine("OnPostAsync() was called");
            
            // Log all values in Event to see what's being received
            Console.WriteLine("Form Data:");
            Console.WriteLine($"Name: {Event.Name}");
            Console.WriteLine($"EventID: {Event.EventID}");
            Console.WriteLine($"Images: {Event.Images}");
            Console.WriteLine($"Description: {Event.Description}");
            Console.WriteLine($"Type: {Event.Type}");
            Console.WriteLine($"Day: {Event.Day}");
            Console.WriteLine($"Month: {Event.Month}");
            Console.WriteLine($"Year: {Event.Year}");
            Console.WriteLine($"Time: {Event.Time}");
            Console.WriteLine($"VenueName: {Event.VenueName}");
            Console.WriteLine($"VenueAddress: {Event.VenueAddress}");
            Console.WriteLine($"OrganizerName: {Event.OrganizerName}");
            Console.WriteLine($"OrganizerContact: {Event.OrganizerContact}");
            Console.WriteLine($"FollowLink: {Event.FollowLink}");
            Console.WriteLine($"TicketPrice: {Event.TicketPrice}");
            Console.WriteLine($"Currency: {Event.Currency}");
            Console.WriteLine($"EventLink: {Event.EventLink}");

            // Remove validation for user-related fields (if applicable)
            ModelState.Remove("Name");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Time");
            ModelState.Remove("Month");

            if (!ModelState.IsValid)
            {
                // If model validation fails, reload the lists
                Console.WriteLine("ModelState is not valid. Reloading lists...");

                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                AllEvents = _context.Events.ToList();
                AllUsers = _context.AppUsers.ToList();
                AllMessages = _context.ContactMessages.ToList();
                TempData["EventSaved"] = false;
                return Page(); // Return the same page
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
                OrganizerContact = Event.OrganizerContact,
                FollowLink = Event.FollowLink,
                TicketPrice = Event.TicketPrice,
                Currency = Event.Currency,
                EventLink = Event.EventLink,
                Images = Event.Images,
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
                Console.WriteLine($"Error saving event: {ex.Message}");
                TempData["EventSaved"] = false;
                return Page(); // Return the same page on error
            }
        }




        
        public IActionResult OnPostUpdateEvent()
        {
            Console.WriteLine("OnPostUpdateEvent() was called");
            Console.WriteLine($"UpdateEvent.Name: {UpdateEvent.Name}");
            Console.WriteLine($"UpdateEvent.EventID: {UpdateEvent.EventID}");

            // Log all values in UpdateEvent to see what's being received
            Console.WriteLine("Form Data:");
            Console.WriteLine($"Name: {UpdateEvent.Name}");
            Console.WriteLine($"EventID: {UpdateEvent.EventID}");
            Console.WriteLine($"Images: {UpdateEvent.Images}");
            Console.WriteLine($"Description: {UpdateEvent.Description}");
            Console.WriteLine($"Type: {UpdateEvent.Type}");
            Console.WriteLine($"Day: {UpdateEvent.Day}");
            Console.WriteLine($"Month: {UpdateEvent.Month}");
            Console.WriteLine($"Year: {UpdateEvent.Year}");
            Console.WriteLine($"Time: {UpdateEvent.Time}");
            Console.WriteLine($"VenueName: {UpdateEvent.VenueName}");
            Console.WriteLine($"VenueAddress: {UpdateEvent.VenueAddress}");
            Console.WriteLine($"OrganizerName: {UpdateEvent.OrganizerName}");
            Console.WriteLine($"OrganizerContact: {UpdateEvent.OrganizerContact}");
            Console.WriteLine($"FollowLink: {UpdateEvent.FollowLink}");
            Console.WriteLine($"TicketPrice: {UpdateEvent.TicketPrice}");
            Console.WriteLine($"Currency: {UpdateEvent.Currency}");
            Console.WriteLine($"EventLink: {UpdateEvent.EventLink}");

            // Remove validation for user-related fields (Name, Email, Password, etc.)
            ModelState.Remove("Name");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Month");
            ModelState.Remove("Time");

            if (!ModelState.IsValid)
            {
                // If model validation fails, reload the lists
                Console.WriteLine("ModelState is not valid. Reloading lists...");

                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                AllEvents = _context.Events.OrderByDescending(e => e.CreatedAt).ToList();
                AllUsers = _context.AppUsers.OrderBy(u => u.CreatedAt).ToList();
                AllMessages = _context.ContactMessages.OrderBy(cm => cm.SentAt).ToList();
                return Page(); // Return the same page
            }

            // Find the existing event in the database
            var existingEvent = _context.Events.Find(UpdateEvent.EventID);
            if (existingEvent != null)
            {
                Console.WriteLine("Event found. Updating values...");

                // Update the event's properties with the new values from the form
                _context.Entry(existingEvent).CurrentValues.SetValues(UpdateEvent);
                Console.WriteLine("Changes applied. Saving changes...");

                // Save changes to the database
                try
                {
                    _context.SaveChanges();
                    Console.WriteLine("Event updated successfully.");
                    TempData["EventUpdated"] = true; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during SaveChanges: {ex.Message}");
                    TempData["EventUpdated"] = false;
                }
            }
            else
            {
                Console.WriteLine("Event not found with the given ID.");
                TempData["EventUpdated"] = false;
            }

            return RedirectToPage(); // Redirect to the same page after update
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
