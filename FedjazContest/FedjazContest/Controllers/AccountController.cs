using FedjazContest.Entities;
using FedjazContest.Models;
using FedjazContest.Services;
using FedjazContest.Models.Settings;
using FedjazContest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FedjazContest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailService emailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IWebHostEnvironment environment, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.environment = environment;
            this.emailService = emailService;
        }

        public async Task<IActionResult> Index(string? username = null)
        {
            ApplicationUser? user = null;

            if(username != null)
            {
                user = await userManager.FindByNameAsync(username);
            }
            else if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                user = await userManager.FindByNameAsync(User.Identity.Name);
            }

            if(user != null)
            {
                return View(user);
            }
            else
            {
                return NotFound();
            }
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
                    EmailConfirmed = false,
                    EmailConfirmationCode = await emailService.ConfirmEmail(registrationModel.Email)
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

                return View("ConfirmEmail", false);
            }
            else
            {
                return View(registrationModel);
            }
        }

        public async Task<IActionResult> ConfirmEmail(string code)
        {
            ApplicationUser? user = dbContext.Users.FirstOrDefault(user => user.EmailConfirmationCode == code && !user.EmailConfirmed);

            if(user != null)
            {
                user.EmailConfirmed = true;
                dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return View("ConfirmEmail", true);
            }
            else
            {
                return NotFound();
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
                Path.Combine(environment.WebRootPath, "images", "avatar.png");
                
                return File(@"~/images/avatar.png", "image/png");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize()]
        [Route("{controller}/{action}/{section=Account}")]
        public async Task<IActionResult> Settings(string section)
        {
            ViewBag.LeftSection = "SettingsLeftSection";
            ViewBag.LeftSectionArguments = new { active = section };

            if(User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            if(section.ToUpper() == "ACCOUNT")
            {
                AccountModel model = new AccountModel(user.FirstName, user.LastName, user.Bio);
                return View("SettingsAccount", model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettingsAccount([FromForm]AccountModel model)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                if(model.Bio != null)
                {
                    user.Bio = model.Bio;
                }

                if(model.Avatar != null && Tools.ImageConverter.TryImageToBase64(model.Avatar, 800, out string result))
                {
                    Image image = new Image(result);
                    dbContext.Images.Add(image);
                    await dbContext.SaveChangesAsync();
                    user.ImageId = image.Id;
                }
                dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }

            ViewBag.LeftSection = "SettingsLeftSection";
            ViewBag.LeftSectionArguments = new { active = "Account" };
            return View("SettingsAccount", model);
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
