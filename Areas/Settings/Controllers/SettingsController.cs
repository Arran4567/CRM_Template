using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Bicks.Areas.Settings.ViewModels;
using Bicks.Data;
using Bicks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Bicks.Entities;

namespace Bicks.Areas.Settings.Controllers
{
    [Authorize(Roles = Role.Superadmin)]
    [Area("Settings")]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SettingsController(ILogger<SettingsController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Settings/UserAccounts")]
        public IActionResult UserAccounts()
        {
            UserAccountRolesViewModel userAccountRolesViewModel = new UserAccountRolesViewModel()
            {
                Users = _userManager.Users.ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            return View(userAccountRolesViewModel);
        }

        [HttpGet]
        [Route("Settings/UserAccounts/CreateUserRole")]
        public IActionResult CreateUserRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Settings/UserAccounts/CreateUserRole")]
        public async Task<IActionResult> CreateUserRole(CreateUserRoleViewModel createUserRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserRoleViewModel);
            }
            else
            {
                if (_roleManager.RoleExistsAsync(createUserRoleViewModel.RoleName).Result)
                {
                    ModelState.AddModelError("", string.Format("{0} already exists", createUserRoleViewModel.RoleName));
                    return View(createUserRoleViewModel);
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole(createUserRoleViewModel.RoleName));
                    return RedirectToAction("UserAccounts");
                }
            }
        }

        [Route("Settings/UserAccounts/DeleteUserRole/{id}")]
        public async Task<IActionResult> DeleteUserRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction("UserAccounts");
        }

        [HttpGet]
        [Route("Settings/UserAccounts/CreateUserAccount")]
        public IActionResult CreateUserAccount()
        {
            ViewBag.AvailableUserRoles = GetUserRolesSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Settings/UserAccounts/CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccount(CreateUserAccountViewModel createUserAccountViewModel)
        {
            ViewBag.AvailableUserRoles = GetUserRolesSelectList();
            if (!ModelState.IsValid)
            {
                return View(createUserAccountViewModel);
            }
            else
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    UserName = createUserAccountViewModel.Name,
                    Email = createUserAccountViewModel.Email,
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, createUserAccountViewModel.Password);
                if (result.Succeeded)
                {
                    foreach (string roleId in createUserAccountViewModel.UserRoles)
                    {
                        IdentityRole role = _roleManager.FindByIdAsync(roleId).Result;
                        await _userManager.AddToRoleAsync(newUser, role.Name);
                    }

                    return RedirectToAction("EditUserAccount", new { id = newUser.Id });
                }
                else
                {
                    _logger.LogError("Account creation failed");
                    foreach (IdentityError error in result.Errors)
                    {
                        _logger.LogError("Account creation error ({0}): {1}", error.Code, error.Description);
                    }
                    ModelState.AddModelError("", "Account creation failed");
                    return View(createUserAccountViewModel);
                }
            }
        }

        [Route("Settings/UserAccounts/EditUserAccount/{id}")]
        public IActionResult EditUserAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("UserAccounts");
            }

            ViewBag.AvailableUserRoles = GetUserRolesSelectList();

            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            IList<string> roles = _userManager.GetRolesAsync(user).Result;

            EditUserAccountViewModel editUserAccountViewModel = new EditUserAccountViewModel()
            {
                ID = user.Id,
                Name = user.UserName,
                Email = user.Email,
                AccountDisabled = user.LockoutEnabled,
                UserRoles = roles
            };

            return View(editUserAccountViewModel);
        }

        [HttpPost]
        [Route("Settings/UserAccounts/EditUserAccount/{id}")]
        public async Task<IActionResult> EditUserAccount(EditUserAccountViewModel editUserAccountViewModel)
        {
            ViewBag.AvailableUserRoles = GetUserRolesSelectList();
            if (!ModelState.IsValid)
            {
                return View(editUserAccountViewModel);
            }
            else
            {
                ApplicationUser user = _userManager.FindByIdAsync(editUserAccountViewModel.ID).Result;

                user.Email = editUserAccountViewModel.Email;
                user.UserName = editUserAccountViewModel.Name;
                user.LockoutEnabled = editUserAccountViewModel.AccountDisabled;

                await _userManager.UpdateAsync(user);

                IList<string> currentRoles = _userManager.GetRolesAsync(user).Result;

                foreach (string role in currentRoles)
                {
                    if (!editUserAccountViewModel.UserRoles.Contains(role))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                foreach (string role in editUserAccountViewModel.UserRoles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                
                await _signInManager.SignInAsync(user, false, null);

                return RedirectToAction("UserAccounts");
            }
        }

        private List<SelectListItem> GetUserRolesSelectList()
        {
            List<SelectListItem> roleSelectList = new List<SelectListItem>();
            List<IdentityRole> roles = _roleManager.Roles.ToList();

            foreach (IdentityRole role in roles)
            {
                roleSelectList.Add(new SelectListItem(role.Name, role.Id));
            }

            return roleSelectList;
        }
    }
}
