using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorApplicationModel : PageModel
    {

        private readonly ApplicationContext _context;
        [BindProperty]
        public Application application { get; set; }
        public List<SelectListItem> selectChildSertificates = new List<SelectListItem>();
        public List<SelectListItem> selectHealthCertificates = new List<SelectListItem>();
        public List<SelectListItem> selectDisabledCertificates= new List<SelectListItem>();
        public List<SelectListItem> selectDeclaration = new List<SelectListItem>();
        [BindProperty]
        public List<ChildCertificate> childCertificates { get; set; }
        [BindProperty]
        public List<DisabledPersonCertificate> disabledCertificates { get; set; }
        [BindProperty]
        public List<HealthCertificate> healthCertificates { get; set; }
        [BindProperty]
        public List<IncomePropertyDeclaration> declarations { get; set; }
        [BindProperty]
        public List<FamilyMember> familyCert { get; set; }
        [BindProperty]
        public Visitor visitor { get; set; }
        public List<string> subsidies = new List<string>();
        static string connectString;
        static NpgsqlConnection connection;
        [BindProperty]
        public List<SelectListItem> subTypes { get; set; }
        public string Login { get; set; }
        public string Message { get; set; }
        public int No { get; set; }
        [BindProperty]
        public int ApplicationHealthNo { get; set; } 
        [BindProperty]
        public List<int> ApplicationChildNo { get; set; } = new List<int>();
        [BindProperty]
        public List<int> ApplicationDisabledNo { get; set; } = new List<int>();
        [BindProperty]
        public int DeclarationNo { get; set; }
        public string sql;
        public bool IsChecked { get; set; }
        public SelectList TagOptions { get; set; }
        public DateTime date { get; set; }

        public VisitorApplicationModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(int id, string login)
        {
            Login = login;
            No = id;
            date = DateTime.Now;
            connectString = $"Host=localhost;Username={Login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            visitor = _context.visitor.FromSqlInterpolated($"select * from visitor where visitor_no = {id}").SingleOrDefault();
            subTypes = new List<SelectListItem> {
                new SelectListItem { Value = "income level", Text = "державну соціальну допомогу малозабезпеченним сім'ям" },
                new SelectListItem { Value = "child care", Text = "державну допомогу сім'ям з дітьми" },
                new SelectListItem { Value = "disability grant", Text = "державну допомогу інвалідам I, II та III групи" },
                new SelectListItem { Value = "disability care", Text = "державну допомогу на догляд особи з інвалідністю" } 
            };
            childCertificates = _context.childcertificate.FromSqlInterpolated($"select * from childcertificate where owner_no = {id};").ToList();
            healthCertificates = _context.healthcertificate.FromSqlInterpolated($"select * from healthcertificate where owner_no = {id};").ToList();
            disabledCertificates = _context.disabledpersoncertificate.FromSqlInterpolated($"select * from disabledpersoncertificate where guardian_no = {id};").ToList();
            declarations = _context.incomepropertydeclaration.FromSqlInterpolated($"select * from incomepropertydeclaration where owner_no = {id}").ToList();
            foreach(var dec in declarations)
            {
                familyCert = _context.familymemberscertificate.FromSqlInterpolated($"select * from familymemberscertificate where declaration_no = {dec.declaration_no}").ToList();
                selectDeclaration.Add(new SelectListItem { Value = $"{dec.declaration_no}", Text = $"Декларація: внесено інформацію про {familyCert.Count} членів родини" });
            }
            foreach (var certificate in childCertificates)
            {
                selectChildSertificates.Add(new SelectListItem { Value = $"{certificate.certificate_no}", Text = $"{certificate.f_name} {certificate.patronymic} {certificate.l_name} {certificate.date_of_birth.ToString("yyyy-MM-dd")}" });
            }
            foreach (var certificate in healthCertificates)
            {
                selectHealthCertificates.Add(new SelectListItem { Value = $"{certificate.certificate_no}", Text = $"{certificate.last_inspection.ToString("yyyy-MM-dd")} {certificate.period_start.ToString("yyyy-MM-dd")} {certificate.period_end.ToString("yyyy-MM-dd")} {certificate.disability_group} {certificate.comment}" });
            }
            foreach (var certificate in disabledCertificates)
            {
                selectDisabledCertificates.Add(new SelectListItem { Value = $"{certificate.certificate_no}", Text = $"{certificate.f_name} {certificate.patronymic} {certificate.l_name} {certificate.date_of_birth.ToString("yyyy-MM-dd")} {certificate.disability_group}" });
            }
            return Page();
        }
        public IActionResult OnPost(int id)
        {
            connection.Open();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            application.comment = "-";
            sql = $"insert into application(owner_no, application_date, application_subsidy_type, status, \"comment\") values " +
                $"('{id}', \'{date}\', \'{application.application_subsidy_type}\', 'filed', \'{application.comment}\');";
            var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            if(application.application_subsidy_type == "income level")
            {
                sql = $"update incomepropertydeclaration set application_no = {FindLastApplicationNo(id)} where declaration_no = {DeclarationNo};";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            else if (application.application_subsidy_type == "disability grant")
            {
                sql = $"update incomepropertydeclaration set application_no = {FindLastApplicationNo(id)} where declaration_no = {DeclarationNo};" +
                    $"update healthcertificate set application_no = {FindLastApplicationNo(id)} where certificate_no = {ApplicationHealthNo};";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            else if (application.application_subsidy_type == "child care")
            {
                sql = $"update incomepropertydeclaration set application_no = {FindLastApplicationNo(id)} where declaration_no = {DeclarationNo};";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                foreach (int num in ApplicationChildNo)
                {
                    sql = $"update childcertificate set application_no = {FindLastApplicationNo(id)} where certificate_no = {num};";
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (application.application_subsidy_type == "disability care")
            {
                sql = $"update incomepropertydeclaration set application_no = {FindLastApplicationNo(id)} where declaration_no = {DeclarationNo};";
                cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                foreach (int num in ApplicationDisabledNo)
                {
                    sql = $"update disabledpersoncertificate set application_no = {FindLastApplicationNo(id)} where certificate_no = {num};";
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            connection.Close();
            return RedirectToPage(); 

        }
        public string FindLastApplicationNo(int? id)
        {
            sql = $"select max(application_no) from application where owner_no = \'{id}\'";
            var cmd = new NpgsqlCommand(sql, connection);
            return cmd.ExecuteScalar().ToString();
        }
    }
}
