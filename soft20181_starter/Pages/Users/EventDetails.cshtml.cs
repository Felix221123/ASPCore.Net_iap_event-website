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
using System.Text.Json;

namespace soft20181_starter.Pages.Users {
    public class EventDetailsModel : PageModel
    {
        private readonly EventAppDbContext _context;
        public EventDetailsModel(EventAppDbContext context)
        {
            _context = context;
        }

        public Event Event { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Event = _context.Events.FirstOrDefault(e => e.EventID == id);
            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }

        public class ImageObj
        {
            public string url { get; set; }
            public string alt { get; set; }
        }

        public User? GetUserFromSession()
        {
            var sessionToken = HttpContext.Session.GetString("SessionToken");
            if (string.IsNullOrEmpty(sessionToken))
                return null; // If there's no session token, return null

            return _context.AppUsers.FirstOrDefault(u => u.SessionToken == sessionToken);
        }

        public async Task<IActionResult> OnPostReserveAsync(Guid id)
        {
            var user = GetUserFromSession(); // You already have this in your ProfileModel, reuse it here.
            if (user == null)
            {
                return RedirectToPage("/Users/Login");
            }

            var existingAttendee = _context.Attendees
                .FirstOrDefault(a => a.UserID == user.UserID && a.EventID == id);

            if (existingAttendee != null)
            {
                TempData["Message"] = "You have already reserved a spot.";
                return RedirectToPage(new { id });
            }

            var attendee = new Attendee
            {
                UserID = user.UserID,
                EventID = id,
                TicketGenerated = true,
                RegisteredAt = DateTime.UtcNow
            };
            _context.Attendees.Add(attendee);

            // Generate ticket
            var ticket = new Ticket
            {
                UserID = user.UserID,
                EventID = id,
                TicketCode = Guid.NewGuid().ToString()
            };
            _context.Tickets.Add(ticket);

            // Update attendee count
            var evt = await _context.Events.FindAsync(id);
            evt.AttendeeCount++;

            await _context.SaveChangesAsync();

            TempData["ReservationSuccess"] = true;
            return RedirectToPage(new { id = id }); // Stay on the same event page

        }

    }
}