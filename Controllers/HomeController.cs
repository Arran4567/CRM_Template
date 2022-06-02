using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bicks.Models;
using Bicks.ViewModels;
using Bicks.Services;
using Bicks.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bicks.Library.Mail;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Bicks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        private readonly IFileTemplateService _fileTemplateService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ApplicationDbContext context, IMailService mailService, IFileTemplateService fileTemplateService, ILogger<HomeController> logger,
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mailService = mailService;
            _logger = logger;
            _fileTemplateService = fileTemplateService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendExampleEmailButton()
        {
            SendExampleEmail("john", DateTime.Now.ToString(), "Leaking Tap", "£20", "Fix", "NC1001", "nwl");
            return RedirectToAction("Index", "Home");
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

        private void SendExampleEmail(string FirstName, string quoteTime, string problem, string totalCost, string solution, string refnumb, string url)
        {
            string body = _fileTemplateService.getExampleEmailBody(FirstName, quoteTime, problem, totalCost, solution, refnumb, url);
            Mail mail = new Mail
            {
                ToEmail = "arran.jones@clickfix.co",
                Subject = "This is an Example email",
                Body = body,
                Attachments = null
            };
            _mailService.SendEmailNow(mail);
        }
    }
}
