using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QueriesToDBTest.Pages.Employee
{
    public class EmployeeStartModel : PageModel
    {
        public string Login { get; set; }
        public void OnGet(string login)
        {
            Login = login;
        }
    }
}
