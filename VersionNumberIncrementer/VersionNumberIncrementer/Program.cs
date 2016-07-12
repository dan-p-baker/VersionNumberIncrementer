using System;
using System.IO;
using System.Text.RegularExpressions;
using VersionNumberIncrementer.Domain;
using VersionNumberIncrementer.Domain.VersionService;

namespace VersionNumberIncrementer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var currentVersion = File.ReadAllText("../../ProductInfo.Txt");

            Console.WriteLine($"Current version number is {currentVersion}");
            Console.WriteLine("Please enter a release type...");

            var readLine = Console.ReadLine();
            if (readLine == null)
                return;

            var command = Regex.Replace(readLine, @"\s+", string.Empty);

            Release.ReleaseTypeEnum releaseType;

            if (Enum.TryParse(command, out releaseType))
            {
                var release = new Release(currentVersion, (int) releaseType);
                var strategy = GetVersionStrategyForReleaseType(release.ReleaseType);
                var releaseService = new ReleaseService(strategy);
                releaseService.IncrementVersionNumber(release);
                releaseService.WriteVersionNumberToFile(release);
                Console.WriteLine($"Release of type: {command} created. The current version number is now {release.VersionNumber}");
                Console.WriteLine("Please enter a release type if you wish to update the version number again...");
                Console.Read();
            }
            else
            {
                Console.WriteLine($"Unknown Command {command}. Options are \"Bug Fix\" or \"Feature\"");
                Console.Read();
            }
        }

        private static IVersionNumberStrategy GetVersionStrategyForReleaseType(Release.ReleaseTypeEnum releaseType)
        {
            switch (releaseType)
            {
                case Release.ReleaseTypeEnum.BugFix:
                    return new MinorVersionNumberStrategy();
                case Release.ReleaseTypeEnum.Feature:
                    return new MajorVersionNumberStrategy();
                default:
                    throw new ArgumentOutOfRangeException(nameof(releaseType), releaseType, null);
            }
        }
    }
}