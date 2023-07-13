using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace QueriesToDBTest.Pages.Admin
{
    public class AdministratorAccountsModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Staff> staff { get; set; }
        public string Login { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public AdministratorAccountsModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=administrator;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            staff = _context.staff.AsNoTracking().ToList();
            return Page();
        }
        public IActionResult OnPost(string login, int id)
        {
            var staffMember = _context.staff.Find(id);
            var staffUser = _context.users.Find(staffMember.login);

            if (staffMember != null)
            {
                connection.Open();
                string sql = $"drop role {staffMember.login};";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                _context.users.Remove(staffUser);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
