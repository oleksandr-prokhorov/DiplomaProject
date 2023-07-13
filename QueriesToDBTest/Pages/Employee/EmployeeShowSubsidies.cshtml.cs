using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Employee
{
    public class EmployeeShowSubsidiesModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public List<Subsidy> subsidies { get; set; }
        [BindProperty]
        public List<Subsidy> temp { get; set; }
        public string Login { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public int Id { get; set; }
        public EmployeeShowSubsidiesModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(int id, string login)
        {
            Id = id;
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=employee;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            temp = _context.subsidy.FromSqlInterpolated($"select * from subsidy where visitor_no = {id} and status = \'active\'").ToList();
            subsidies = _context.subsidy.FromSqlInterpolated($"select * from subsidy where visitor_no = {id} and status != \'active\'").ToList();
        }
        public IActionResult OnPostCancel(int no)
        {
            connection.Open();
            string sql = $"update subsidy set status = \'canceled\' where payment_no = {no};";
            var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToPage();

        }
    }
}
