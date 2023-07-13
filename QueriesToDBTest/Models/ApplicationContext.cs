using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Visitor> visitor { get; set; }
        public DbSet<Subsidy> subsidy { get; set; }
        public DbSet<FamilyMember> familymemberscertificate { get; set; }
        public DbSet<IncomePropertyDeclaration> incomepropertydeclaration { get; set; }
        public DbSet<Application> application { get; set; }
        public DbSet<ChildCertificate> childcertificate { get; set; }
        public DbSet<DisabledPersonCertificate> disabledpersoncertificate { get; set; }
        public DbSet<HealthCertificate> healthcertificate { get; set; }
        public DbSet<IncomeFamily> incomefamily { get; set; }
        public DbSet<SpendingFamily> spendingfamily { get; set; }
        public DbSet<PropertyFamily> propertyfamily { get; set; }
        public DbSet<LandFamily> landfamily { get; set; }
        public DbSet<VehiclesFamily> vehiclesfamily { get; set; }
        public DbSet<Staff> staff { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
