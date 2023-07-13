using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorShowApplicationModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public List<ChildCertificate> childCertificates { get; set; }
        [BindProperty]
        public List<DisabledPersonCertificate> disabledCertificates { get; set; }
        [BindProperty]
        public List<HealthCertificate> healthCertificates { get; set; }
        [BindProperty]
        public IncomePropertyDeclaration declaration { get; set; }
        [BindProperty]
        public Application? application { get; set; }
        public int No { get; set; }
        public string Login { get; set; }
        // 
        [BindProperty]
        public List<FamilyMember> familyMembersCertificate { get; set; } = new List<FamilyMember>();
        [BindProperty]
        public List<IncomeFamily> incomeFamilyRecords { get; set; } = new List<IncomeFamily>();
        [BindProperty]
        public List<SpendingFamily> spendingFamilyRecords { get; set; } = new List<SpendingFamily>();
        [BindProperty]
        public List<PropertyFamily> propertyFamilyRecords { get; set; } = new List<PropertyFamily>();
        [BindProperty]
        public List<LandFamily> landFamilyRecords { get; set; } = new List<LandFamily>();
        [BindProperty]
        public List<VehiclesFamily> vehiclesFamilyRecords { get; set; } = new List<VehiclesFamily>();
        public List<string> NameIncome = new List<string>();

        public List<string> SurnameIncome = new List<string>();
        public List<string> PatronymicIncome = new List<string>();
        public List<string> NameSpending = new List<string>();
        public List<string> SurnameSpending = new List<string>();
        public List<string> PatronymicSpending = new List<string>();
        public List<string> NameProperty = new List<string>();
        public List<string> SurnameProperty = new List<string>();
        public List<string> PatronymicProperty = new List<string>();
        public List<string> NameLand = new List<string>();
        public List<string> SurnameLand = new List<string>();
        public List<string> PatronymicLand = new List<string>();
        public List<string> NameVehicles = new List<string>();
        public List<string> SurnameVehicles = new List<string>();
        public List<string> PatronymicVehicles = new List<string>();
        public string ApplicationStatus { get; set; }
        public string Message { get; set; }
        public string Comment { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public VisitorShowApplicationModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(int id, string login)
        {
            No = id;
            Login = login;
            connectString = $"Host=localhost;Username={Login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            application = _context.application.FromSqlInterpolated($"select * from application where owner_no = {id} and status = 'filed'").OrderBy(q => q.application_no).LastOrDefault();
            if(application == null)
            {
                Message = "На жаль, зараз у вас немає поданих заяв";
                return Page();
            }
            if(application.status == "filed")
            {
                ApplicationStatus = "заява на розгляданні";
            }
            else if (application.status == "approved")
            {
                ApplicationStatus = "заяву схвалено";
            }
            else
            {
                ApplicationStatus = "заяву відхилено";
            }
            Comment = application.comment;
            declaration = _context.incomepropertydeclaration.FromSqlInterpolated($"select * from incomepropertydeclaration where application_no = {application.application_no}").SingleOrDefault();
            familyMembersCertificate = _context.familymemberscertificate.FromSqlInterpolated($"select * from familymemberscertificate where declaration_no = {declaration.declaration_no}").ToList();
            foreach (var member in familyMembersCertificate)
            {
                IncomeFamily? temp_income = _context.incomefamily.FromSqlInterpolated($"select * from incomefamily where member_no = {member.record_no}").SingleOrDefault();
                if (temp_income != null)
                {
                    incomeFamilyRecords.Add(temp_income);
                    SurnameIncome.Add(member.l_name);
                    NameIncome.Add(member.f_name);
                    PatronymicIncome.Add(member.patronymic);
                }
                SpendingFamily? temp_spending = _context.spendingfamily.FromSqlInterpolated($"select * from spendingfamily where member_no = {member.record_no}").SingleOrDefault();
                if (temp_spending != null)
                {
                    spendingFamilyRecords.Add(temp_spending);
                    SurnameSpending.Add(member.l_name);
                    NameSpending.Add(member.f_name);
                    PatronymicSpending.Add(member.patronymic);
                }
                PropertyFamily? temp_property = _context.propertyfamily.FromSqlInterpolated($"select * from propertyfamily where member_no = {member.record_no}").SingleOrDefault();
                if (temp_property != null)
                {
                    propertyFamilyRecords.Add(temp_property);
                    SurnameProperty.Add(member.l_name);
                    NameProperty.Add(member.f_name);
                    PatronymicProperty.Add(member.patronymic);
                }
                LandFamily? temp_land = _context.landfamily.FromSqlInterpolated($"select * from landfamily where member_no = {member.record_no}").SingleOrDefault();
                if (temp_land != null)
                {
                    landFamilyRecords.Add(temp_land);
                    SurnameLand.Add(member.l_name);
                    NameLand.Add(member.f_name);
                    PatronymicLand.Add(member.patronymic);
                }
                VehiclesFamily? temp_vehicles = _context.vehiclesfamily.FromSqlInterpolated($"select * from vehiclesfamily where member_no = {member.record_no}").SingleOrDefault();
                if (temp_vehicles != null)
                {
                    vehiclesFamilyRecords.Add(temp_vehicles);
                    SurnameVehicles.Add(member.l_name);
                    NameVehicles.Add(member.f_name);
                    PatronymicVehicles.Add(member.patronymic);
                }
            }
            childCertificates = _context.childcertificate.FromSqlInterpolated($"select * from childcertificate where application_no = {application.application_no}").ToList();
            healthCertificates = _context.healthcertificate.FromSqlInterpolated($"select * from healthcertificate where application_no = {application.application_no}").ToList();
            disabledCertificates = _context.disabledpersoncertificate.FromSqlInterpolated($"select * from disabledpersoncertificate where application_no = {application.application_no}").ToList();
            return Page();
        }
        public IActionResult OnPost(int id, string login)
        {
            connection.Open();
            string sql = $"update incomepropertydeclaration set application_no = null where application_no = {application.application_no};";
            var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            if(childCertificates.Count > 1)
            {
                foreach(var cert in childCertificates)
                {
                    sql = $"update childcertificate set application_no = null where application_no = {application.application_no};";
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            if (healthCertificates.Count > 1)
            {
                foreach (var cert in healthCertificates)
                {
                    sql = $"update healthcertificate set application_no = null where application_no = {application.application_no};";
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            if (disabledCertificates.Count > 1)
            {
                foreach (var cert in disabledCertificates)
                {
                    sql = $"update disabledpersoncertificate set application_no = null where application_no = {application.application_no};";
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            connection.Close();
            _context.application.Remove(application);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
