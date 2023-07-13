using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class IncomeFamily
    {
        [Key]
        public int record_no { get; set; }
        public int member_no { get; set; }
        public string income_type { get; set; }
        
        public decimal amount { get; set; }
        public string source_of_income { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime income_start { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime income_finish { get; set; }
    }
}
