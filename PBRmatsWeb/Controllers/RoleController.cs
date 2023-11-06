﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PBRmatsWeb.Controllers
{
    [Authorize(Roles = "RootAdmin")]
    public class RoleController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Implement your action methods to change roles for ASP.NET users here
        // For example, you can have a method to add a user to a role, remove from a role, etc.

        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role does not exist.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            // Redirect to a relevant view or action
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role does not exist.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            // Redirect to a relevant view or action
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var model = (users, roles, _userManager);

            return View(model);
        }
    }
}