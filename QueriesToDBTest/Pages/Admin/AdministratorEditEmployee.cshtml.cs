using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace QueriesToDBTest.Pages.Admin
{
    public class AdministratorEditEmployeeModel : PageModel
    {
        private readonly ApplicationContext _context;
        public string Login { get; set; }
        public static string OldStaffLogin { get; set; }
        [BindProperty]
        public Staff? staffMember { get; set; }
        [BindProperty]
        public User? user { get; set; }
        public string Category = "employee";
        static string connectString;
        static NpgsqlConnection connection;
        public AdministratorEditEmployeeModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(int id, string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=administrator;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            staffMember = _context.staff.Find(id);
            if(staffMember == null)
            {
                return NotFound();
            }
            OldStaffLogin = staffMember.login;
            user = _context.users.Find(staffMember.login);
            return Page();
        }
        public IActionResult OnPost(int id, string login)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            connection.Open();
            string sql;
            if (OldStaffLogin != staffMember.login)
            {
                sql = $"alter role {OldStaffLogin} rename to {staffMember.login};";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            if(user.password != null)
            {
                sql = $"update users set password = \'{user.password}\' where login = \'{OldStaffLogin}\';";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            sql = $"update users set login = \'{staffMember.login}\' where login = \'{OldStaffLogin}\';" +
                $"update staff set f_name=\'{staffMember.f_name}\', patronymic=\'{staffMember.patronymic}\', l_name=\'{staffMember.l_name}\', " +
                $"position =\'{staffMember.position}\' where staff_no = {id};";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToPage("AdministratorAccounts", new { login });
        }
    }
}
