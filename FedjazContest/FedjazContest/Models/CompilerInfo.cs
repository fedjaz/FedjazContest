namespace FedjazContest.Models
{
    public class CompilerInfo
    {
        public string Language { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public List<string> Versions { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public IEnumerable<string> SystemNames
        {
            get
            {
                foreach(string version in Versions)
                {
                    yield return $"{Language}.{version}";
                }
            }
        }

        public IEnumerable<string> DisplayVersions
        {
            get
            {
                foreach(string version in Versions)
                {
                    yield return $"{DisplayName} - {version}";
                }
            }
        }

        public IEnumerable<(string, string)> DisplayNamesAndValues
        {
            get
            {
                IEnumerator<string> display = DisplayVersions.GetEnumerator();
                IEnumerator<string> values = SystemNames.GetEnumerator();
                while (display.MoveNext() && values.MoveNext())
                {
                    yield return (display.Current, values.Current);
                }
            }
        }
        public CompilerInfo(string language, string beautifulName, List<string> versions, string description)
        {
            Language = language;
            Versions = versions;
            Description = description;
            DisplayName = beautifulName;
        }
    }
}
