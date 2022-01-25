using FedjazContest.Entities;
using FedjazContest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = registrationModel.FirstName,
                    LastName = registrationModel.LastName,
                    UserName = registrationModel.Username,
                    Email = registrationModel.Email,
                    EmailConfirmed = true,
                };
                
                await userManager.CreateAsync(user, registrationModel.Password);
                await userManager.AddToRoleAsync(user, "user");
                await signInManager.SignInAsync(user, true);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(registrationModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckUsername(string Username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(Username);
            return Json(user == null);
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(Email);
            return Json(user == null);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
