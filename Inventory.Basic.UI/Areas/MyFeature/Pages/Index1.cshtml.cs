using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Basic.UI.Areas.MyFeature.Pages
{
    public class Index1Model : PageModel
    {
        public Inventory.Basic.UI.MyFeature.Pages.Page1Model pagemodel1 = new Inventory.Basic.UI.MyFeature.Pages.Page1Model();

        public void OnGet()
        {
            pagemodel1.strMgs = "from index RCL";
        }
    }
}
