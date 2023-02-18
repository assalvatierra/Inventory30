using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InvWeb.Data;
using InvWeb.Services.PredefinedReports;

namespace InvWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Viewer(
            [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName)
        {

            //var reportToOpen = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
            var reportToOpen = "ItemList";

            var model = new Models.ViewerModel
            {
                ViewerModelToBind = await clientSideModelGenerator.GetModelAsync(reportToOpen, WebDocumentViewerController.DefaultUri)
            };
            return View(model);
        }

    }
}
