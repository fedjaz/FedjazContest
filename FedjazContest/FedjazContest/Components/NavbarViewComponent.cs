using FedjazContest.Models;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private List<NavbarItem> items = new List<NavbarItem>
        {
            new NavbarItem("Home", "Home"),
            new NavbarItem("Account", "Account"),
            new NavbarItem("Contests", "Contests"),
        };
        public IViewComponentResult Invoke()
        {
            string? controller = ViewContext.RouteData.Values["controller"] as string;
            if(controller != null)
            {
                foreach(NavbarItem item in items)
                {
                    if(item.Controller == controller)
                    {
                        item.IsActive = true;
                        break;
                    }
                }
            }

            return View(items);
        }
    }
}
