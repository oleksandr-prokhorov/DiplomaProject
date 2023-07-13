using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QueriesToDBTest.Models
{
    public class FamilyMember
    {
        [Key]
        public int record_no { get; set; }
        public int? declaration_no { get; set; }
        [Required(ErrorMessage = "Введіть ім'я члена родини")]
        public string f_name { get; set; }
        [Required(ErrorMessage = "Введіть по-батькові члена родини")]
        public string patronymic { get; set; }
        [Required(ErrorMessage = "Введіть призівище члена родини")]
        public string l_name { get; set; }
        [Required(ErrorMessage = "Вкажіть ступінь спорідненості члена родини")]
        public string degree_relationship { get; set; }
        [Required(ErrorMessage = "Вкажіть номер документу члена родини")]
        public string idn { get; set; }
        public string? comment { get; set; }
    }
}
