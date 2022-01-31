using FedjazContest.Entities;
using FedjazContest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Components
{
    public class AccountViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountViewComponent(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? controller = ViewContext.RouteData.Values["controller"] as string;
            string? action = ViewContext.RouteData.Values["action"] as string;
            if (controller != null && action != null && controller == "Account" &&
                (action == "Login" || action == "Register"))
            {
                return Content("");
            }

            AccountComponentModel accountComponentModel;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);
                accountComponentModel = new AccountComponentModel(user);
            }
            else
            {
                accountComponentModel = new AccountComponentModel(false, "/");
            }

            return View(accountComponentModel);
        }
    }
}
