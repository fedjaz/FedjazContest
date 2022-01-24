using FedjazContest.Models;
using Microsoft.AspNetCore.Mvc;

namespace FedjazContest.Components
{
    public class AccountViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            AccountComponentModel accountComponentModel;
            if (true)
            {
                accountComponentModel = new AccountComponentModel(false, "/");
            }
            else
            {
                accountComponentModel = null;
            }

            return View(accountComponentModel);
        }
    }
}
