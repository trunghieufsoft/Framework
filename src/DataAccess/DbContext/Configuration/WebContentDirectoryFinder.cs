using System;
using System.IO;
using System.Linq;

namespace DataAccess.DbContext.Configuration
{
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            string appSettingsFile = @"appsettings.json";
            string coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(DataAccessModule).Assembly.Location);
            if (File.Exists(Path.Combine(coreAssemblyDirectoryPath, appSettingsFile)))
            {
                return coreAssemblyDirectoryPath;
            }

            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("Could not find location of App assembly!");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "Framework.sln"))
            {
                directoryInfo = directoryInfo.Parent ?? throw new Exception("Could not find content root folder!");
            }

            string webHostFolder = Path.Combine(directoryInfo.FullName, "WebClient");
            if (Directory.Exists(webHostFolder))
            {
                return webHostFolder;
            }

            throw new Exception("Could not find root folder of the web project!");
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}
