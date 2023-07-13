using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
    public class IncomePropertyDeclaration
    {
        [Key]
        public int declaration_no { get; set; }
        public int? application_no { get; set; }
        public int owner_no { get; set; }
        public string? document_no { get; set; }
        public DateTime? date_of_issue { get; set; }
    }
}
