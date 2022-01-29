using Microsoft.AspNetCore.Mvc;
using FedjazContest.Models;

namespace FedjazContest.Components
{
    public class SettingsLeftSectionViewComponent : ViewComponent
    {
        private readonly List<SettingsLeftSectionModel> sections = new List<SettingsLeftSectionModel>
        {
            new SettingsLeftSectionModel("Account", "Account"),
            new SettingsLeftSectionModel("Password", "Password"),
            new SettingsLeftSectionModel("Preferences", "Preferences"),

        };

        public async Task<IViewComponentResult> InvokeAsync(string active)
        {
            if(active != null)
            {
                foreach(SettingsLeftSectionModel section in sections)
                {
                    if(section.SectionName == active)
                    {
                        section.IsActive = true;
                    }
                }
            }
            return View(sections);
        }
    }
}
