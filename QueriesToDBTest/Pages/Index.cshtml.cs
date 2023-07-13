using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using QueriesToDBTest.Models;
using Npgsql;
namespace QueriesToDBTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        static string connectString = "Host=localhost;Username=postgres;Password=gilbert;Database=DiplomaProject";
        [TempData]
        public string login { get; set; }
        public List<User> users { get; set; }
        public string Message { get; set; } = "Введіть ім'я та пароль облікового запису";
        static NpgsqlConnection connection = new NpgsqlConnection(connectString);

        public IndexModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            users = _context.users.AsNoTracking().ToList();
        }

        public IActionResult OnPost(string login, string password)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = _context.users.FromSqlInterpolated($"SELECT * FROM users where login={login}").SingleOrDefault();

            connection.Open();
            string sql = $"select convert_from(encode(digest(\'{password}\'::bytea, 'sha256'), 'base64')::bytea, 'utf-8');";
            var cmd = new NpgsqlCommand(sql, connection);
            string digestPassword = cmd.ExecuteScalar().ToString();
            connection.Close();

            if (user == null || digestPassword != user.password)
            {
                Message = $"Введено неправильне ім'я облікового запису та/або пароль"; 
                return Page();
            }
            else if (digestPassword == user.password)
            {
                login = user.login;
                if (user.category == "employee")
                {
                    return RedirectToPage("Employee/EmployeeStart", new { login });
                }
                else if (user.category == "visitor")
                {
                    return RedirectToPage("Client/VisitorStart", new { login });
                }
                else if (user.category == "administrator")
                {
                    return RedirectToPage("Admin/Administrator", new { login });
                }
            }
            return Page();
        }
    }
}