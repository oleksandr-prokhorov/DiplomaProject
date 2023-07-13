using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorDocumentsModel : PageModel
    {
        private readonly ApplicationContext _context;
        public Visitor visitor { get; set; }
        public int Id { get; set; }
        public string Login { get; set; }
        public VisitorDocumentsModel(ApplicationContext db)
        {
            _context = db;
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            Id = (int)id;
            visitor = await _context.visitor.FindAsync(id);
            Login = visitor.login;
            return Page();
        }
    }
}
