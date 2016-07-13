using System;
using System.Text.RegularExpressions;
using VersionNumberIncrementer.Domain;

namespace VersionNumberIncrementer
{
    internal class Program
    {
        private static IFileService _fileService;

        private static void Main()
        {
            BootstrapApplication();

            var currentVersion = _fileService.ReadVersionNumberFromFile();

            WriteWelcomeMessageToConsole(currentVersion);

            var readLine = Console.ReadLine();
            if (readLine == null)
                return;

            var command = Regex.Replace(readLine, @"\s+", string.Empty);

            Release.ReleaseTypeEnum releaseType;

            if (Enum.TryParse(command, out releaseType))
            {
                var release = new Release(currentVersion);
                release = release.IncrementVersion(releaseType);
                _fileService.WriteVersionNumberToFile(release);

                WriteVersionNumberUpdatedMessageToConsole(release);
                Console.Read();
            }
            else
            {
                WriteUnknownCommandEnteredMessageToConsole(command);
                Console.Read();
            }
        }

        private static void WriteWelcomeMessageToConsole(string currentVersion)
        {
            Console.WriteLine($"Hello, the current version number is {currentVersion}");
            Console.WriteLine("To update the version number, please enter a release type. (Options are \"Bug Fix\" or \"Feature\")");
        }

        private static void WriteUnknownCommandEnteredMessageToConsole(string command)
        {
            Console.WriteLine($"{command} is an unknown command. Options are \"Bug Fix\" or \"Feature\"");
            Console.WriteLine("Press enter to close the application.");
        }

        private static void WriteVersionNumberUpdatedMessageToConsole(Release release)
        {
            Console.WriteLine($"Thank you, the version number is now {release.Version}");
            Console.WriteLine("Press enter to close the application.");
        }

        private static void BootstrapApplication()
        {
            Bootstrap.Start();
            _fileService = Bootstrap.Container.GetInstance<IFileService>();
        }
    }
}