using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bicks.Areas.Invoicing.ViewModels;
using Bicks.Areas.Invoicing.Data.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Bicks.Models;
using Bicks.Controllers;
using Rotativa.AspNetCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Hangfire;
using Newtonsoft.Json;

namespace Bicks.Areas.Invoicing.Controllers
{
    [Area("Invoicing")]
    public class InvoicingController : Controller
    {
        private readonly ILogger<InvoicingController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private InvoicingWorkUnit _workUnit;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public InvoicingController(ILogger<InvoicingController> logger, UserManager<ApplicationUser> userManager, InvoicingWorkUnit workUnit,
            IWebHostEnvironment hostEnvironment, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _userManager = userManager;
            _workUnit = workUnit;
            _hostEnvironment = hostEnvironment;
            _backgroundJobClient = backgroundJobClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateInvoice()
        {
            SalesInvoiceViewModel salesInvoiceViewModel = _workUnit.GenerateExampleInvoice();
            TempData["salesInvoiceViewModel"] = JsonConvert.SerializeObject(salesInvoiceViewModel);
            return RedirectToAction("GenerateInvoice");
        }

        public IActionResult GenerateInvoice()
        {
            SalesInvoiceViewModel salesInvoiceViewModel = JsonConvert.DeserializeObject<SalesInvoiceViewModel>(TempData["salesInvoiceViewModel"].ToString());
            string filename = salesInvoiceViewModel.InvoiceNo.ToString("000000") + ".pdf";
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string directory = System.IO.Path.Combine(wwwRootPath, "Invoices");
            string filepath = System.IO.Path.Combine(directory, filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            ControllerContext controllerContext = new ControllerContext(this.ControllerContext);
            ViewAsPdf viewAsPdf = new ViewAsPdf("SalesInvoiceTemplate", salesInvoiceViewModel) { FileName = "test.pdf" };
            var pdfAsByte = viewAsPdf.BuildFile(controllerContext).Result;
            System.IO.File.WriteAllBytes(filepath, pdfAsByte);
            return RedirectToAction("Index");
        }
    }
}
