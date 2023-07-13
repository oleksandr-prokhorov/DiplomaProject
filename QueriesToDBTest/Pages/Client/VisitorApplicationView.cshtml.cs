using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorApplicationViewModel : PageModel
    {
        public int No { get; set; }
        public string Login { get; set; }
        public void OnGet(int id, string login)
        {
            No = id;
            Login = login;
        }
    }
}
