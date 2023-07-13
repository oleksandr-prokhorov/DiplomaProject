using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorHealthCertificateModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public HealthCertificate healthCertificate { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public string[] disability_groups = new[] { "I", "II", "III" };
        string sql;
        public VisitorHealthCertificateModel (ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(int? id)
        {
            Id = (int)id;
            var visitor = _context.visitor.FromSqlInterpolated($"select * from visitor where visitor_no = {Id}").SingleOrDefault();
            connectString = $"Host=localhost;Username={visitor.login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
        }
        public IActionResult OnPost(int? id)
        {
            Id = (int)id;
            if (ModelState.IsValid)
            {
                connection.Open();
                sql = $"insert into healthcertificate(last_inspection, period_start, period_end, owner_no, disability_group, \"comment\") " +
                    $"values(\'{healthCertificate.last_inspection.ToString("yyyy-MM-dd")}\', \'{healthCertificate.period_start.ToString("yyyy-MM-dd")}\', " +
                    $"\'{healthCertificate.period_end.ToString("yyyy-MM-dd")}\', {Id}, \'{healthCertificate.disability_group}\', \'{healthCertificate.comment}\')";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                Message = $"Інформацію успішно внесено до системи";
                return Page();
            }
            return Page();
        }
    }
}
