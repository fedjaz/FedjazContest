using FedjazContest.Entities;
using FedjazContest.Models;
using FedjazContest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment environment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm]LoginModel loginModel)
        {
            ApplicationUser user = await userManager.FindByNameAsync(loginModel.EmailOrUsername);
            if(user == null)
            {
                user = await userManager.FindByEmailAsync(loginModel.EmailOrUsername);
            }

            if(user == null)
            {
                ModelState.AddModelError(nameof(loginModel.EmailOrUsername), "There is no user with this Email or Username");
            }
            else if(!await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                ModelState.AddModelError(nameof(loginModel.Password), "Password is incorrect");
            }
            else if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(nameof(loginModel.EmailOrUsername), "Please confirn your email before entering your account");
            }


            if (ModelState.IsValid && user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: true);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(loginModel);
            }
        } 

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegistrationModel registrationModel)
        {
            if (!await CheckUsername(registrationModel.Username))
            {
                ModelState.AddModelError(nameof(registrationModel.Username), "This Username is already in use");
            }

            if (!await CheckEmail(registrationModel.Email))
            {
                ModelState.AddModelError(nameof(registrationModel.Email), "This Email is already in use");
            }

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
                
                if(registrationModel.Avatar != null)
                {
                    if(Tools.ImageConverter.TryImageToBase64(registrationModel.Avatar, 800, out string result))
                    {
                        Image image = new Image(result);
                        dbContext.Images.Add(image);
                        await dbContext.SaveChangesAsync();

                        user.ImageId = image.Id;
                    }
                }

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

        public async Task<IActionResult> GetAvatar(string? userId)
        {
            ApplicationUser? user = null;
            if(userId == null)
            {
                if(User.Identity != null && User.Identity.IsAuthenticated)
                {
                    user = await userManager.FindByNameAsync(User.Identity.Name);
                }
            }
            else
            {
                user = await userManager.FindByIdAsync(userId);
            }
            
            if(user == null)
            {
                return NotFound();
            }

            Image? avatar = dbContext.Images.FirstOrDefault(img => img.Id == user.ImageId);
            if(avatar != null)
            {
                MemoryStream stream = Tools.ImageConverter.Base64ToImage(avatar.Base64);
                return File(stream.ToArray(), "image/png");
            }
            else
            {
                return File(Path.Combine(environment.WebRootPath, "images", "avatar.png"), "image/png");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> CheckUsername(string username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            return user == null;
        }

        private async Task<bool> CheckEmail(string email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            return user == null;
        }
    }
}
