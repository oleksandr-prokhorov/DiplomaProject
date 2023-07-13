using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Pages;
using Npgsql;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorStartModel : PageModel
    {
        static string connectString;
        static NpgsqlConnection connection;
        public string Login { get; set; }
        public int No { get; set; }
        public void OnGet(string login)
        {
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            No = GetVisitorNo();
        }
        public int GetVisitorNo()
        {
            connection.Open();
            int n;
            string sql = $"select visitor_no from visitor where login=\'{Login}\'";
            var cmd = new NpgsqlCommand(sql, connection);
            n = Convert.ToInt32(cmd.ExecuteScalar());
            connection.Close();
            return n;
        }
    }
}
