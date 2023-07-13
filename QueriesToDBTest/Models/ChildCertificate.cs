using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class ChildCertificate
    {
        [Key]
        public int certificate_no { get; set; }
        public string f_name { get; set; }
        public string patronymic { get; set; }
        public string l_name { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime date_of_birth { get; set; }
        public int? application_no { get; set; }
        public int owner_no { get; set; }
        public string? document_no { get; set; }
        public string? date_of_issue { get; set; }
    }
}
