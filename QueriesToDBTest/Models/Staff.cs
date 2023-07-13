using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class Staff
    {
        [Key]
        public int staff_no { get; set; }
        public string f_name { get; set; }
        public string patronymic { get; set; }
        public string l_name { get; set; }
        public string position { get; set; }
        public string login { get; set; }
    }
}
