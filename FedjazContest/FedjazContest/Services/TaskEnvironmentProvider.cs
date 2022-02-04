using FedjazContest.Models;

namespace FedjazContest.Services
{
    public class TaskEnvironmentProvider : ITaskEnvironmentProvider
    {

        public IEnumerable<CompilerInfo> GetAvailableCompilers()
        {
            return new List<CompilerInfo>
            {
                new CompilerInfo("csharp", "C#", new List<string>{ ".net6.0", ".net5.0", "mono" }, "Microsoft .Net C# compiler"),
                new CompilerInfo("cpp", "C++", new List<string>{ "g++", "g++11", "g++17" }, "GNU GCC Compiler"),
                new CompilerInfo("python", "Python", new List<string>{ "2.7", "3.8"}, "Python default interpreter")
            };
        }

        public IEnumerable<string> GetAvailableLanguages()
        {
            return new List<string>
            {
                "english",
                "russian",
            };
        }

        public IEnumerable<(string, string)> GetCompilersValues()
        {
            foreach(CompilerInfo compiler in GetAvailableCompilers())
            {
                foreach((string, string) value in compiler.DisplayNamesAndValues)
                {
                    yield return value;
                }
            }
        }
    }
}
