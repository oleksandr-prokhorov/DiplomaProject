using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace QueriesToDBTest.Pages.Admin
{
    public class AdministratorCreateEmployeeModel : PageModel
    {
        private readonly ApplicationContext _context;
        public string Login { get; set; }
        public static string OldStaffLogin { get; set; }
        [BindProperty]
        public Staff staffMember { get; set; }
        [BindProperty]
        public User? user { get; set; }
        public string Category = "employee";
        static string connectString;
        static NpgsqlConnection connection;
        public AdministratorCreateEmployeeModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=administrator;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            return Page();
        }
        public IActionResult OnPost(string login)
        {
            Login = login;
            if (ModelState.IsValid)
            {
                connection.Open();
                string sql = $"create role {staffMember.login} with login createrole password \'employee\'; " +
                $"grant employee to {staffMember.login}; " +
                $"insert into users values(\'{staffMember.login}\',\'{user.password}\','employee'); " +
                $"update users set \"password\" = 'password' where login = \'{staffMember.login}\'; " +
                $"update users set \"password\" = \'{user.password}\' where login = \'{staffMember.login}\'; " +
                $"insert into staff(f_name, patronymic, l_name, position, login) " +
                $"values(\'{staffMember.f_name}\',\'{staffMember.patronymic}\',\'{staffMember.l_name}\',\'{staffMember.position}\',\'{staffMember.login}\');";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return RedirectToPage("AdministratorAccounts", new { Login });
            }
            return Page();
        }
    }
}
