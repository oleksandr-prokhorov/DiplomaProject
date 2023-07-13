using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class SpendingFamily
    {
        [Key]
        public int record_no { get; set; }
        public int member_no { get; set; }
        public string spending_purpose { get; set; }

        public decimal cost { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime date_of_purchase { get; set; }
    }
}
