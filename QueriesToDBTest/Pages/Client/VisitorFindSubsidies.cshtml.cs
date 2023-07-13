using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Npgsql;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorFindSubsidiesModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public List<Subsidy> subsidy { get; set; }
        public List<Subsidy> temp { get; set; }
        public string Login { get; set; }
        [BindProperty, DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime date_start { get; set; }
        [BindProperty, DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime date_end { get; set; }
        [BindProperty]
        public string subsidy_status { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public string[] types = new[] { "active", "canceled", "finished" };
        Visitor visitor { get; set; }
        string sql { get; set; }
        public VisitorFindSubsidiesModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(int? id)
        {
            subsidy = _context.subsidy.AsNoTracking().ToList();
            date_start = DateTime.Now;
            date_end = DateTime.Now;
            Id = (int)id;
            connectString = $"Host=localhost;Username={Login};Password=visitor;Database=DiplomaProject";
            visitor = _context.visitor.Find(id);
            Login = visitor.login;
            connection = new NpgsqlConnection(connectString);
            subsidy = _context.subsidy.FromSqlInterpolated($"SELECT * FROM subsidy where visitor_no = {Id}").ToList();
            return Page();
        }
        public IActionResult OnPostFindBetweenDate(int? id)
        {
            Id = (int)id;
            if (subsidy == null)
            {
                return NotFound();
            }
            subsidy = _context.subsidy.FromSqlInterpolated($"SELECT * FROM subsidy where visitor_no = {Id} and (period_start between ({date_start.ToString("yyyy-MM-dd")})::date and {date_end.ToString("yyyy-MM-dd")}::date or period_end between {date_start.ToString("yyyy-MM-dd")}::date and {date_end.ToString("yyyy-MM-dd")}::date)").ToList();
            Message = Convert.ToString(date_start);
            return Page();
        }
        public IActionResult OnPostFindByStatus(int? id)
        {
            date_start = DateTime.Now;
            date_end = DateTime.Now;
            Id = (int)id;
            if (subsidy == null)
            {
                return NotFound();
            }
            subsidy = _context.subsidy.FromSqlInterpolated($"SELECT * FROM subsidy where visitor_no = {Id} and status = ({subsidy_status}::subsidy_status)").ToList();
            Message = subsidy_status;
            return Page();
        }

    }
}
