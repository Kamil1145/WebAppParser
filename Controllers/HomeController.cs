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

            ViewData["Address"] = ViewDataSort.Sort("Adres");
            ViewData["Title"] = ViewDataSort.Sort("Tytul");
            ViewData["Content"] = ViewDataSort.Sort("Opis");
            ViewData["Prize"] = ViewDataSort.Sort("Cena");
            ViewData["Rooms"] = ViewDataSort.Sort("Liczba pokoi");
            ViewData["Floor"] = ViewDataSort.Sort("Poziom");
            ViewData["Area"] = ViewDataSort.Sort("Powierzchnia");
            ViewData["Fee"] = ViewDataSort.Sort("Czynsz(dodatkowo)");



            return View("Property");
        }


    }
}
 