using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UiClass
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

     

        public UiClass.ViewModels.Person person;
        public void OnGet()
        {
            this.person = new UiClass.ViewModels.Person
                ("Abel", "Phone:0917####");


        }
    }
}