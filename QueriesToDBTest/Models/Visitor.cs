using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
    public class Visitor
    {
        [Key]
        public int visitor_no { get; set; }
        public string f_name { get; set; }
        public string patronymic { get; set; }
        public string l_name { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime date_of_birth { get; set; }
        public int assigned_to { get; set; }
        public string passport_no { get; set; }
        public string idn { get; set; }
        public string login { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
    }
}
