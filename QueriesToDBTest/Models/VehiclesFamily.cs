using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace QueriesToDBTest.Models
{
    public class VehiclesFamily
    {
        [Key]
        public int record_no { get; set; }
        public int member_no { get; set; }
        public string car_brand { get; set; }

        public string car_license_plate { get; set; }
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime year_of_issue { get; set; }
    }
}
