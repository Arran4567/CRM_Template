using Bicks.Areas.Invoicing.Controllers;
using Bicks.Areas.ProductManagement.Data.DAL;
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
using Bicks.Areas.ProductManagement.ViewModels;

namespace Bicks.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    public class ProductManagementController : Controller
    {
        private readonly ILogger<ProductManagementController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ProductManagementWorkUnit _workUnit;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ProductManagementController(ILogger<ProductManagementController> logger, UserManager<ApplicationUser> userManager, ProductManagementWorkUnit workUnit,
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

        public IActionResult ProductList()
        {
            return View(_workUnit.ProductRepository.Get(orderBy: pr => pr.OrderBy(p => p.Category.Name).ThenBy(p => p.Name)));
        }

        public IActionResult CreateProduct()
        {
            ProductViewModel editProductViewModel = new ProductViewModel()
            {
                Categories = _workUnit.CategoryRepository.Get().ToList()
            };
            return View(editProductViewModel);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel editProductViewModel)
        { 
            Category category = _workUnit.CategoryRepository.GetByID(editProductViewModel.Product.Category.ID);
            editProductViewModel.Product.Category = category;
            _workUnit.ProductRepository.Insert(editProductViewModel.Product);
            _workUnit.Save();
            return RedirectToAction("ProductList");
        }

        public IActionResult EditProduct(int id)
        {
            ProductViewModel editProductViewModel = new ProductViewModel()
            {
                Product = _workUnit.ProductRepository.GetByID(id),
                Categories = _workUnit.CategoryRepository.Get().ToList(),
            };
            return View(editProductViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel editProductViewModel)
        {
            Category category = _workUnit.CategoryRepository.GetByID(editProductViewModel.Product.Category.ID);
            editProductViewModel.Product.Category = category;
            _workUnit.ProductRepository.Update(editProductViewModel.Product);
            _workUnit.Save();
            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int id)
        {
            Product product = _workUnit.ProductRepository.GetByID(id);
            _workUnit.ProductRepository.Delete(product);
            _workUnit.Save();
            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(_workUnit.CategoryRepository.Get(orderBy: cr => cr.OrderBy(c => c.Name)));
        }
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category) 
        { 
            _workUnit.CategoryRepository.Insert(category);
            _workUnit.Save();
            return RedirectToAction("CategoryList");
        }

        public IActionResult EditCategory(int id)
        {
            return View(_workUnit.CategoryRepository.GetByID(id));
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            _workUnit.CategoryRepository.Update(category);
            _workUnit.Save();
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteCategory(int id)
        {
            Category category = _workUnit.CategoryRepository.GetByID(id);
            _workUnit.CategoryRepository.Delete(category);
            _workUnit.Save();
            return RedirectToAction("CategoryList");
        }
    }
}
