namespace FedjazContest.Models
{
    public class SettingsLeftSectionModel
    {
        public SettingsLeftSectionModel(string title, string sectionName)
        {
            Title = title;
            SectionName = sectionName;
        }

        public string Title { get; set; } = "";
        public string SectionName { get; set; } = "";
        public bool IsActive { get; set; } = false;
    }
}
