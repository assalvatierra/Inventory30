using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InvWeb.Data;
using InvWeb.Services.PredefinedReports;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using System.Collections.Generic;
using System.Security.Policy;
using System;

namespace InvWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Designer(
            [FromServices] IReportDesignerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName)
        {
            InvWeb.Reporting.Models.ReportDesignerCustomModel model = new InvWeb.Reporting.Models.ReportDesignerCustomModel();
            model.ReportDesignerModel = await CreateDefaultReportDesignerModel(clientSideModelGenerator, reportName, null);
            return View(model);
        }

        public static Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            // Create a SQL data source with the specified connection string.
            SqlDataSource ds = new SqlDataSource("ReportsMssqlServer");
            // Create a SQL query to access the Products data table.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("InvCategories").SelectAllColumnsFromTable().Build("Reports");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            dataSources.Add("InventoryDB", ds);
            return dataSources;
        }

        public static async Task<ReportDesignerModel> CreateDefaultReportDesignerModel(IReportDesignerClientSideModelGenerator clientSideModelGenerator, string reportName, XtraReport report)
        {
            reportName = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
            var dataSources = GetAvailableDataSources();
            if (report != null)
            {
                return await clientSideModelGenerator.GetModelAsync(report, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            }
            return await clientSideModelGenerator.GetModelAsync(reportName, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
        }


        public async Task<IActionResult> Viewer(
            [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName, [FromQuery] string param)
        {
            var reportToOpen = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;

            var model = new Models.ViewerModel
            {
                ViewerModelToBind = await clientSideModelGenerator.GetModelAsync(reportToOpen, WebDocumentViewerController.DefaultUri)
            }; 

            if(!string.IsNullOrEmpty(param))
            {
                ////url sample: https://localhost:44359/Home/Viewer?reportName=Item-Search&param=Id:123;Type:REL
                string[] paramvalues = param.Split(";");
                foreach(string paramvalue in paramvalues)
                {
                    if(paramvalue.Split(':').Length>1)
                    {
                        string pName = paramvalue.Split(':')[0];
                        string pValue = paramvalue.Split(":")[1];

                        if(model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters.Length>0)
                        {
                            for(int i = 0; i < model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters.Length; i++)
                            {
                                if (model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[i].Name.ToLower()==pName.ToLower())
                                {
                                    model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[i].Value = pValue;
                                    model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[i].ValueInfo = pValue;
                                }

                            }
                        }

                    }
                    else
                    {
                        string errMsg = "Parameter format incorrect";
                    }
                }


            }

            return View(model);
        }

    }
}
