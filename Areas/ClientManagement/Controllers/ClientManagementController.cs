using Bicks.Areas.ClientManagement.Data.DAL;
using Bicks.Models;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Areas.ClientManagement.Controllers
{
    [Area("ClientManagement")]
    public class ClientManagementController : Controller
    {
        private readonly ILogger<ClientManagementController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ClientManagementWorkUnit _workUnit;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ClientManagementController(ILogger<ClientManagementController> logger, UserManager<ApplicationUser> userManager, ClientManagementWorkUnit workUnit,
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

        public IActionResult ClientList()
        {
            return View(_workUnit.ClientManagementRepository.Get(orderBy: cmr => cmr.OrderBy(c => c.Name)));
        }

        public IActionResult CreateClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateClient(Client client)
        {
            _workUnit.ClientManagementRepository.Insert(client);
            _workUnit.Save();
            return RedirectToAction("ClientList");
        }

        public IActionResult EditClient(int id)
        {
            Client client = _workUnit.ClientManagementRepository.GetByID(id);
            return View(client);
        }

        [HttpPost]
        public IActionResult EditClient(Client client)
        {
            _workUnit.ClientManagementRepository.Update(client);
            _workUnit.Save();
            return RedirectToAction("ClientList");
        }

        public IActionResult DeleteClient(int id)
        {
            Client client= _workUnit.ClientManagementRepository.GetByID(id);
            _workUnit.ClientManagementRepository.Delete(client);
            _workUnit.Save();
            return RedirectToAction("ClientList");
        }
    }
}
