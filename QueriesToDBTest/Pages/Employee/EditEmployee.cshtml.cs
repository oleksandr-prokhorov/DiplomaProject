using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace QueriesToDBTest.Pages.Employee
{
    public class EditEmployeeModel : PageModel
    {
        private readonly ApplicationContext _context;

        public string Login { get; set; }
        // [BindProperty]
        public User user { get; set; }
        [BindProperty]
        public Visitor visitor { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public string Ñat = "visitor";
        public string OldLogin { get; set; }
        public EditEmployeeModel(ApplicationContext db)
        {
            _context = db;
        }
        public async Task<IActionResult> OnGetAsync(int? id, string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=employee;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            if (id == null)
            {
                return NotFound();
            }
            visitor = await _context.visitor.FindAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }
            OldLogin = visitor.login;
            return Page();
        }
        public IActionResult OnPostAsync(string login)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _context.Attach(visitor).State = EntityState.Modified;
            string date = visitor.date_of_birth.ToString("yyyy-MM-dd");
            try
            {
                connection.Open();
                visitor.assigned_to = GetStaffNo(login);
                string sql = $"update visitor " +
                $"set f_name=\'{visitor.f_name}\', patronymic=\'{visitor.patronymic}\', l_name=\'{visitor.l_name}\', date_of_birth=\'{date}\', " +
                $"passport_no=\'{visitor.passport_no}\', idn =\'{visitor.idn}\', address = \'{visitor.address}\', mobile = \'{visitor.mobile}\' " +
                $"where visitor_no = {visitor.visitor_no};";
                // $"update users set login = \'{visitor.login}\', password = \'{user.password}\' where login = \'{OldLogin}\'";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!_context.visitor.Any(e => e.visitor_no == visitor.visitor_no))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Login = login;
            return RedirectToPage("Employee", new { Login });
        }
        public int GetStaffNo(string login)
        {
            string sql = $"select staff_no from staff where \'{login}\' = login";
            var cmd = new NpgsqlCommand(sql, connection);
            return Convert.ToInt32(cmd.ExecuteScalar().ToString());
        }
    }
}
