using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppParser.Models;

namespace WebAppParser.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
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

        public ActionResult Property()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string message)
        {
            Parser.ScrapeData(message);
            ViewData.Model = Models.Property.propertyD;
            ViewData["Test"] = Models.Property.propertyD;
            ViewData["Address"] = Models.Property.propertyD["Adres"];
            return View("Property");
        }
    }
}
 