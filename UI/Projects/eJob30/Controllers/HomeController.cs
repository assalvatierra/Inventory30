using eJob30.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CoreLib.Interfaces.System;

namespace eJob30.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISystemServices systemservices;

        public HomeController(ILogger<HomeController> logger, ISystemServices _sysservices)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modules()
        {
            var sysItems = this.systemservices.getServices(0).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}