using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorShowCertificatesModel : PageModel
    {
        public readonly ApplicationContext _context;
        public int Size = 5;
        public List<HealthCertificate> healthCertificates { get; set; }
        public List<ChildCertificate> childCertificates { get; set; }
        public List<DisabledPersonCertificate> disabledPersonCertificates { get; set; }
        [BindProperty]
        public IncomePropertyDeclaration declaration { get; set; }
        public List<IncomePropertyDeclaration> currDeclaration = new List<IncomePropertyDeclaration>();
        [BindProperty]
        public List<FamilyMember> familyMembersCertificate { get; set; }
        [BindProperty]
        public List<IncomeFamily> incomeFamilyRecords { get; set; }
        [BindProperty]
        public List<SpendingFamily> spendingFamilyRecords { get; set; }
        [BindProperty]
        public List<PropertyFamily> propertyFamilyRecords { get; set; }
        [BindProperty]
        public List<LandFamily> landFamilyRecords { get; set; }
        [BindProperty]
        public List<VehiclesFamily> vehiclesFamilyRecords { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        public string Login { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        //
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
        public VisitorShowCertificatesModel(ApplicationContext db)
        {
            _context = db;
        }
        public IActionResult OnGet(int id)
        {
            Id = id;
            healthCertificates = _context.healthcertificate.FromSqlInterpolated($"SELECT * FROM healthcertificate where owner_no = {Id}").ToList();
            childCertificates = _context.childcertificate.FromSqlInterpolated($"SELECT * FROM childcertificate where owner_no = {Id}").ToList();
            disabledPersonCertificates = _context.disabledpersoncertificate.FromSqlInterpolated($"SELECT * FROM disabledpersoncertificate where guardian_no = {Id}").ToList();
            declaration = _context.incomepropertydeclaration.FromSqlInterpolated($"select * from incomepropertydeclaration where owner_no = {Id}").FirstOrDefault();
            SurnameIncome = new List<string>();
            NameIncome = new List<string>();
            PatronymicIncome = new List<string>();
            familyMembersCertificate = new List<FamilyMember>();
            incomeFamilyRecords = new List<IncomeFamily>();
            spendingFamilyRecords = new List<SpendingFamily>();
            propertyFamilyRecords = new List<PropertyFamily>();
            landFamilyRecords = new List<LandFamily>();
            vehiclesFamilyRecords = new List<VehiclesFamily>();

            if (declaration == null)
            {
                return Page();
            }
            else
            {
                currDeclaration.Add(declaration);
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
            }
            return Page();

        }
        public async Task<IActionResult> OnPostDeleteHealthAsync(int no)
        {
            var certificate = await _context.healthcertificate.FindAsync(no);

            if (certificate != null)
            {
                _context.healthcertificate.Remove(certificate);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteChildAsync(int no)
        {
            var certificate = await _context.childcertificate.FindAsync(no);

            if (certificate != null)
            {
                _context.childcertificate.Remove(certificate);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteDisabledPersonAsync(int no)
        {
            var certificate = await _context.disabledpersoncertificate.FindAsync(no);

            if (certificate != null)
            {
                _context.disabledpersoncertificate.Remove(certificate);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteDeclarationAsync(int no)
        {
            var declaration = await _context.incomepropertydeclaration.FindAsync(no);

            if (declaration != null)
            {
                _context.incomepropertydeclaration.Remove(declaration);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        public string GetFullName(int member_no)
        {
            return "";
        }
    }
}
