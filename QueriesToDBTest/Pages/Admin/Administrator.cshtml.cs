using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;

namespace QueriesToDBTest.Pages.Admin
{
    public class AdministratorModel : PageModel
    {
        public readonly ApplicationContext _context;
        public string Login { get; set; }
        public int Id { get; set; }
        public AdministratorModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(string login)
        {
            Login = login;
        }
    }
}
