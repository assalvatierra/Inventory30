using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebX.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public Inventory.Basic.UI.MyFeature.Pages.Page1Model pagemodel1 = new Inventory.Basic.UI.MyFeature.Pages.Page1Model();

        public void OnGet()
        {
            pagemodel1.strMgs = "hello from index";

        }
    }
}