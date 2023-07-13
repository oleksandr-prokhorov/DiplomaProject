using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;

namespace QueriesToDBTest.Pages.Employee
{
    public class CreateVisitorModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public Visitor visitor { get; set; }
        [BindProperty]
        public User user { get; set; }
        public string cat = "visitor";
        static string connectString;
        static NpgsqlConnection connection;
        public string Login { get; set; }
        public CreateVisitorModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=employee;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
        }
        public IActionResult OnPostAsync(string login)
        {
            Login = login;
            if (ModelState.IsValid)
            {
                connection.Open();
                visitor.assigned_to = GetStaffNo(Login);
                string date = visitor.date_of_birth.ToString("yyyy-MM-dd");
                user.category = cat;
                string sql = $"create role {visitor.login} with login password \'visitor\'; " +
                $"grant visitor to {visitor.login}; " +
                $"insert into users values(\'{visitor.login}\',\'{user.password}\','visitor'); " +
                $"update users set \"password\" = 'password' where login = \'{visitor.login}\'; " +
                $"update users set \"password\" = \'{user.password}\' where login = \'{visitor.login}\'; " +
                "insert into visitor(f_name,patronymic,l_name,date_of_birth,assigned_to,passport_no,idn, login, mobile, address) " +
                $"values(\'{visitor.f_name}\',\'{visitor.patronymic}\',\'{visitor.l_name}\',\'{date}\',\'{visitor.assigned_to}\', " +
                $"\'{visitor.passport_no}\',\'{visitor.idn}\',\'{visitor.login}\', \'{visitor.mobile}\', \'{visitor.address}\')";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return RedirectToPage("Employee", new { Login });
            }
            return Page();
        }
        public int GetStaffNo(string login)
        {
            string sql = $"select staff_no from staff where \'{login}\' = login";
            var cmd = new NpgsqlCommand(sql, connection);
            return Convert.ToInt32(cmd.ExecuteScalar().ToString());
        }
    }
}
