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
            this.systemservices= _sysservices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modules()
        {
            var sysItems = this.systemservices.getServices(0).ToList();
            ViewBag.sysItems = sysItems;

            return View();
        }
        public IActionResult route(int Id)
        {
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