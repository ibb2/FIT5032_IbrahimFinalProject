using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT5032_IbrahimFinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; 
        public RoleController(RoleManager<IdentityRole> roleManager) { 
            this._roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        public IActionResult Create() { 
            return View(new IdentityRole()); 
        }
        [HttpPost] public async Task<IActionResult> Create(IdentityRole role) { 
            await _roleManager.CreateAsync(role); 
            return RedirectToAction("Index"); 
        }

        public IActionResult DeleteRole()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> OnPostDelete(string roleId)
        {
            var roleToBeDeleted = _roleManager.FindByIdAsync(roleId).Result;

            if (roleToBeDeleted != null) {
                await _roleManager.DeleteAsync(roleToBeDeleted);
            } else
            {
                ViewBag.ErrorMessage = "Role does not exist!";
            }

            return RedirectToAction("Index");
        }


    }
}
