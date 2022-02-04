using FedjazContest.Models;

namespace FedjazContest.Services
{
    public interface ITaskEnvironmentProvider
    {
        public IEnumerable<string> GetAvailableLanguages();
        public IEnumerable<CompilerInfo> GetAvailableCompilers();
        public IEnumerable<(string, string)> GetCompilersValues();
    }
}
