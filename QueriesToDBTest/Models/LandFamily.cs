using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class LandFamily
    {
        [Key]
        public int record_no { get; set; }
        public int member_no { get; set; }
        public double area { get; set; }

        public string type { get; set; }
        public string purpose { get; set; }
    }
}
