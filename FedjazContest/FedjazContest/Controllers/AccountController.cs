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
        private readonly Data.ApplicationDbContext dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Data.ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
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
                await dbContext.SaveChangesAsync();
                await userManager.AddToRoleAsync(user, "user");
                await signInManager.SignInAsync(user, true);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(registrationModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckUsername(string username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            return Json(user == null);
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            return Json(user == null);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
