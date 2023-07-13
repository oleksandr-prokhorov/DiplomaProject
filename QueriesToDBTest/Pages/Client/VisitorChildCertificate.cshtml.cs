using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QueriesToDBTest.Models;
using Npgsql;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorChildCertificateModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public ChildCertificate childCertificate { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        string sql;
        public VisitorChildCertificateModel(ApplicationContext db)
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
                sql = $"insert into childcertificate(date_of_birth, f_name, patronymic, l_name, owner_no) values(\'{childCertificate.date_of_birth.ToString("yyyy-MM-dd")}\', " +
                    $"\'{childCertificate.f_name}\', \'{childCertificate.patronymic}\', \'{childCertificate.l_name}\', '{Id}')";
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
