using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QueriesToDBTest.Pages.Employee
{
    public class AppointSubsidyEmployeeModel : PageModel
    {
        private readonly ApplicationContext _context;
        public string Login { get; set; }
        public List<Visitor> visitor { get; set; }
        public List<Visitor> visitorFiledAplication { get; set; }
        public List<Visitor> visitorApprovedApplication { get; set; }
        static string connectString;
        static NpgsqlConnection connection;

        public AppointSubsidyEmployeeModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(string login)
        {
            connectString = $"Host=localhost;Username={login};Password=employee;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            connection.Open();
            int no = GetStaffNo(login);
            connection.Close();
            Login = login;
            visitor = _context.visitor.FromSqlInterpolated($"select * from visitor where assigned_to = {no};").ToList(); //  and not exists (select * from application where owner_no = visitor_no and status != 'filed')
            visitorFiledAplication = _context.visitor.FromSqlInterpolated($"select * from visitor where assigned_to = {no} and exists (select * from application where owner_no = visitor_no and status = 'filed');").ToList();
            visitorApprovedApplication = _context.visitor.FromSqlInterpolated($"select * from visitor where assigned_to = {no} and exists (select * from application app where (app.owner_no = visitor_no and app.status = 'approved') and not exists(select * from subsidy sub where sub.application_no = app.application_no));").ToList();

            // сотрудник переходит на страницу с посетителями. Несколько кнопок - просмотр истории выплат, просмотр документов, назначение субсидии. В истории выплат наверху расположена
            // активная выплата, которую можно отменить. 
        }
        public int GetStaffNo(string login)
        {
            string sql = $"select staff_no from staff where \'{login}\' = login";
            var cmd = new NpgsqlCommand(sql, connection);
            return Convert.ToInt32(cmd.ExecuteScalar().ToString());
        }
    }
}
