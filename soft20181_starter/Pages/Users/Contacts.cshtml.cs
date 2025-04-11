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


namespace soft20181_starter.Pages.Users {
    public class ContactsModel : PageModel
    {
        private readonly EventAppDbContext _context;
        public ContactsModel(EventAppDbContext context)
        {
            _context = context;
        }
        
    }
}