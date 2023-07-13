using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorDeclarationModel : PageModel
    {
        private readonly ApplicationContext _context;
        public int Size = 5;
        public int Id { get; set; }
        [BindProperty]
        public IncomePropertyDeclaration declaration { get; set; }
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
        public FamilyMember familyMember { get; set; }
        public int MemberId { get; set;}
        public string Message { get; set; }
        [BindProperty]
        public List<string> NameIncome { get; set; }
        [BindProperty]
        public List<string> SurnameIncome { get; set; }
        [BindProperty]
        public List<string> PatronymicIncome { get; set; }
        [BindProperty]
        public List<string> NameSpending { get; set; }
        [BindProperty]
        public List<string> SurnameSpending { get; set; }
        [BindProperty]
        public List<string> PatronymicSpending { get; set; }
        [BindProperty]
        public List<string> NameProperty { get; set; }
        [BindProperty]
        public List<string> SurnameProperty { get; set; }
        [BindProperty]
        public List<string> PatronymicProperty { get; set; }
        [BindProperty]
        public List<string> NameLand { get; set; }
        [BindProperty]
        public List<string> SurnameLand { get; set; }
        [BindProperty]
        public List<string> PatronymicLand { get; set; }
        [BindProperty]
        public List<string> NameVehicles { get; set; }
        [BindProperty]
        public List<string> SurnameVehicles { get; set; }
        [BindProperty]
        public List<string> PatronymicVehicles { get; set; }
        static string connectString;
        static NpgsqlConnection connection;

        public VisitorDeclarationModel(ApplicationContext db)
        {
            _context = db;
            
        }
        public IActionResult OnGet(int id)
        {
            Id = id;
            var visitor = _context.visitor.FromSqlInterpolated($"select * from visitor where visitor_no = {Id}").SingleOrDefault();
            connectString = $"Host=localhost;Username={visitor.login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
            return Page();
        }
        public IActionResult OnPostAdd(int id)
        {
            Id = id;
            declaration.owner_no = Id;
            try
            {
                if (familyMembersCertificate[0].idn != null)
                {
                    _context.incomepropertydeclaration.Add(declaration);
                    _context.SaveChanges();
                    int dec_no = FindLastDeclarationNo(Id);
                    foreach (var record in familyMembersCertificate)
                    {
                        if (record.f_name != null)
                        {
                            record.declaration_no = dec_no;
                            _context.familymemberscertificate.Add(record);
                            _context.SaveChanges();
                            Message = record.l_name;
                        }
                    }
                }
                for (int i = 0; i < Size; i++)
                {
                    if (NameIncome[i] != null && SurnameIncome[i] != null && PatronymicIncome[i] != null)
                    {
                        string sql;
                        FindFamilyMemberId(NameIncome[i], SurnameIncome[i], PatronymicIncome[i]);
                        connection.Open();
                        sql = $"insert into incomefamily(member_no, income_type, amount, source_of_income, income_start, income_finish) values" +
                            $"({MemberId}, \'{incomeFamilyRecords[i].income_type}\', \'{incomeFamilyRecords[i].amount}\', \'{incomeFamilyRecords[i].source_of_income}\', " +
                            $"\'{incomeFamilyRecords[i].income_start.ToString("yyyy-MM-dd")}\', \'{incomeFamilyRecords[i].income_finish.ToString("yyyy-MM-dd")}\');";
                        var cmd = new NpgsqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                for (int i = 0; i < Size; i++)
                {
                    if (NameSpending[i] != null && SurnameSpending[i] != null && PatronymicSpending[i] != null)
                    {
                        string sql;
                        FindFamilyMemberId(NameSpending[i], SurnameSpending[i], PatronymicSpending[i]);
                        connection.Open();
                        sql = $"insert into spendingfamily(member_no, spending_purpose, cost, date_of_purchase) values" +
                            $"({MemberId}, \'{spendingFamilyRecords[i].spending_purpose}\', \'{spendingFamilyRecords[i].cost}\', \'{spendingFamilyRecords[i].date_of_purchase.ToString("yyyy-MM-dd")}\'); ";
                        var cmd = new NpgsqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                for (int i = 0; i < Size; i++)
                {
                    if (NameProperty[i] != null && SurnameProperty[i] != null && PatronymicProperty[i] != null)
                    {
                        string sql;
                        FindFamilyMemberId(NameProperty[i], SurnameProperty[i], PatronymicProperty[i]);
                        connection.Open();
                        sql = $"insert into propertyfamily(member_no, area, person_num, address) values" +
                            $"({MemberId}, \'{propertyFamilyRecords[i].area}\', {propertyFamilyRecords[i].person_num}, \'{propertyFamilyRecords[i].address}\'); ";
                        var cmd = new NpgsqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                for (int i = 0; i < Size; i++)
                {
                    if (NameLand[i] != null && SurnameLand[i] != null && PatronymicLand[i] != null)
                    {
                        string sql;
                        FindFamilyMemberId(NameLand[i], SurnameLand[i], PatronymicLand[i]);
                        connection.Open();
                        sql = $"insert into landfamily(member_no, area, type, purpose) values" +
                            $"({MemberId}, \'{landFamilyRecords[i].area}\', \'{landFamilyRecords[i].type}\', \'{landFamilyRecords[i].purpose}\'); ";
                        var cmd = new NpgsqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                for (int i = 0; i < Size; i++)
                {
                    if (NameVehicles[i] != null && SurnameVehicles[i] != null && PatronymicVehicles[i] != null)
                    {
                        string sql;
                        FindFamilyMemberId(NameVehicles[i], SurnameVehicles[i], PatronymicVehicles[i]);
                        connection.Open();
                        sql = $"insert into Vehiclesfamily(member_no, car_brand, car_license_plate, year_of_issue) values" +
                            $"({MemberId}, \'{vehiclesFamilyRecords[i].car_brand}\', \'{vehiclesFamilyRecords[i].car_license_plate}\', \'{vehiclesFamilyRecords[i].year_of_issue.ToString("yyyy-MM-dd")}\'); ";
                        var cmd = new NpgsqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch
            {
                Message = "Виникла помилка при оформлені декларації";
            }
            
            return RedirectToPage();
        }
        public void FindFamilyMemberId (string name, string surname, string patronymic)
        {
            FamilyMember? member = _context.familymemberscertificate.FromSqlInterpolated($"SELECT * from familymemberscertificate where f_name={name} and l_name={surname} and patronymic={patronymic} and declaration_no = {FindLastDeclarationNo(Id)}").SingleOrDefault();
            if (member != null)
            {
                MemberId = member.record_no;
            }
            else MemberId = 0;
        }
        public int FindLastDeclarationNo(int id)
        {
            IncomePropertyDeclaration? dec = _context.incomepropertydeclaration.FromSqlInterpolated($"select * from incomepropertydeclaration where owner_no = {id}").OrderBy(q => q.declaration_no).LastOrDefault();
            return dec.declaration_no;
        }
    }
}
