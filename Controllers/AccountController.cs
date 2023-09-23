
using Intern2Grow_Auth.Models;
using Intern2Grow_Auth.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Intern2Grow_Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(CreateUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Fullname = userVM.Fullname,
                    UserName = userVM.Username,
                    Email = userVM.Email,
                    Address = userVM.Address,
                    CreatedAt = DateTime.Now,
                };
                var userCreated = await userManager.CreateAsync(user, userVM.Password);
                if (userCreated.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var err in userCreated.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View();
        }

        //GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(userVM.Username);
                if (user != null)
                {
                    var matched = await userManager.CheckPasswordAsync(user, userVM.Password);
                    if (matched)
                    {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "invalid login info");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user not found");
                }
            }
            return View(userVM);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return View("DoesNotExist");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return RedirectToAction("index");
            }
            return View("DoesNotExist");
        }

        [Authorize]
        public async Task<IActionResult> Details()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return View("DoesNotExist");
        }
    }
}
