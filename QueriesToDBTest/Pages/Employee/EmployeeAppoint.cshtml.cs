using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace QueriesToDBTest.Pages.Employee
{
    public class EmployeeAppointModel : PageModel
    {
        private readonly ApplicationContext _context;
        public string Login { get; set; }
        public EmployeeAppointModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(string login)
        {
            Login = login;
            return Page();
        }
    }
}
