using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
    public class Subsidy
    {
        [Key]
        public int payment_no { get; set; }
        public int visitor_no { get; set; }
        public int appointed_by { get; set; }
        public decimal payment_amount { get; set; }
        public string type { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime period_start { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime period_end { get; set; }
        public string comment { get; set; }
        public string status { get; set; }
        public int application_no { get; set; }
    }
}
