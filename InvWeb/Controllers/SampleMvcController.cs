using Microsoft.AspNetCore.Mvc;

namespace InvWeb.Controllers
{
    public class SampleMvcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
