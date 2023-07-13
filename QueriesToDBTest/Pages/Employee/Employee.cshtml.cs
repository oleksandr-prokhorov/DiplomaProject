using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QueriesToDBTest.Pages
{
    public class EmployeeModel : PageModel
    {
        private readonly ApplicationContext _context;
        public List<Visitor> visitor { get; set; }
        public string Login { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public EmployeeModel(ApplicationContext db)
        {
            _context = db;

        }
        public void OnGet(string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=employee;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            visitor = _context.visitor.AsNoTracking().ToList();
            connection.Open();
            int no = GetStaffNo(login);
            connection.Close();
            List<Visitor> temp = new List<Visitor>();
            foreach(var vis in visitor)
            {
                if(vis.assigned_to.Equals(no))
                {
                    temp.Add(vis);
                }
            }
            visitor = temp;
            Login = login;
        }
        public IActionResult OnPostDelete(int id)
        {

            var visitor = _context.visitor.Find(id);
            var visUser = _context.users.Find(visitor.login);
            if (visitor != null)
            {
                connection.Open();
                string sql = $"drop role {visitor.login};";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                _context.users.Remove(visUser);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
        public int GetStaffNo(string login)
        {
            string sql = $"select staff_no from staff where \'{login}\' = login";
            var cmd = new NpgsqlCommand(sql, connection);
            return Convert.ToInt32(cmd.ExecuteScalar().ToString());
        }
    }
}
