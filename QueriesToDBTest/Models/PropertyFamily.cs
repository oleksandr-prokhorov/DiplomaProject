using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class PropertyFamily
    {
        [Key]
        public int record_no { get; set; }
        public int member_no { get; set; }
        public double area { get; set; }

        public int person_num { get; set; }
        public string address { get; set; }
    }
}
