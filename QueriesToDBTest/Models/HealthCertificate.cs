using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class HealthCertificate
    {
        [Key]
        public int certificate_no { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime last_inspection { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime period_start { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime period_end { get; set; }
        public string comment { get; set; }
        public int owner_no { get; set; }
        public int? application_no { get; set; }
        public string disability_group { get; set; }
        public string? document_no { get; set; }
        public string? date_of_issue { get; set; }
    }
}
