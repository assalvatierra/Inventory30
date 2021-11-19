using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.RCLSample.Pages
{
    public class SampleModel : PageModel
    {
        private readonly ILogger<SampleModel> _logger;

        public SampleModel(ILogger<SampleModel> logger)
        {
            _logger = logger;
        }

        public UiClass.ViewModels.Person person;
        public void OnGet()
        {
            this.person = new UiClass.ViewModels.Person
                ("Abel S.", "Phone:0917####");


        }
    }
}