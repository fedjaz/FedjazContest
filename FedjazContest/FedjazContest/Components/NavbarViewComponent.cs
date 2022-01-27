using FedjazContest.Models;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private List<NavbarItem> items = new List<NavbarItem>
        {
            new NavbarItem("Home", "Home"),
            new NavbarItem("About", "Home", "About"),
            new NavbarItem("Creating tasks", "Home", "Creating tasks"),
            new NavbarItem("Contests", "Contests"),
            new NavbarItem("Attempts", "Attempts"),
        };
        public IViewComponentResult Invoke()
        {
            string? controller = ViewContext.RouteData.Values["controller"] as string;
            string? action = ViewContext.RouteData.Values["action"] as string;
            if(controller != null && action != null)
            {
                foreach(NavbarItem item in items)
                {
                    if(item.Controller == controller && item.Action == action)
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
