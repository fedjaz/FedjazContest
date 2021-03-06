using Microsoft.AspNetCore.Mvc;
using FedjazContest.Models;

namespace FedjazContest.Components
{
    public class SettingsLeftSectionViewComponent : ViewComponent
    {
        private readonly List<SettingsLeftSectionModel> sections = new List<SettingsLeftSectionModel>
        {
            new SettingsLeftSectionModel("Account", "Account"),
            new SettingsLeftSectionModel("Security", "Security"),
            new SettingsLeftSectionModel("Preferences", "Preferences"),
        };

        public async Task<IViewComponentResult> InvokeAsync(string active)
        {
            if(active != null)
            {
                foreach(SettingsLeftSectionModel section in sections)
                {
                    if(section.SectionName.ToUpper() == active.ToUpper())
                    {
                        section.IsActive = true;
                        break;
                    }
                }
            }
            return View(sections);
        }
    }
}
