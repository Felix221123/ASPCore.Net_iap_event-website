using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity; // <-- Required for IdentityUser, UserManager, PasswordHasher



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

        [BindProperty]
        public Event Event { get; set; }

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
            AllUsers = _context.AppUsers.OrderBy(u => u.CreatedAt).ToList();


            // Admin is authenticated, proceed to load the dashboard
            return Page();
        }

        // admin logout, by clearing session token
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("AdminSessionToken");
            return RedirectToPage("/Admins/AdminLogIn");
        }


        // save or update event
        public IActionResult OnPostAddEvent()
        {
            System.Diagnostics.Debug.WriteLine("OnPostAddEvent() was called");

            if (!ModelState.IsValid)
            {
                AllEvents = _context.Events.OrderByDescending(e => e.CreatedAt).ToList();
                TempData["EventSaved"] = false;
                return Page();
            }

            Event.EventID = Guid.NewGuid();
            Event.CreatedAt = DateTime.UtcNow;
            _context.Events.Add(Event);
            _context.SaveChanges();

            TempData["EventSaved"] = true;
            return RedirectToPage();
        }

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
