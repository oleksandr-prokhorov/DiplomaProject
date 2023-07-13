using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
    public class Application
    {
        [Key]
        public int application_no { get; set; }
        public DateOnly application_date { get; set; }
        public string application_subsidy_type { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
    }
}
