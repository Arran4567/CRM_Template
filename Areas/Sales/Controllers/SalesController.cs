using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateSale()
        {
            return View();
        }
        public IActionResult EditSale()
        {
            return View();
        }
        public IActionResult DeleteSale()
        {
            return View();
        }
    }
}
