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

        public IActionResult OnGet(int id)
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

        
    }
}