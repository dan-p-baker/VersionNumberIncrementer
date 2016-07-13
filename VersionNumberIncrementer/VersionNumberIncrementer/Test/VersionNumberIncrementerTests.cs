using NUnit.Framework;
using VersionNumberIncrementer.Domain;

namespace VersionNumberIncrementer.Test
{
    public class VersionNumberIncrementerTests
    {
        private static IReleaseService _releaseService;

        [SetUp]
        public void Setup()
        {
            Bootstrap.Start();
            _releaseService = Bootstrap.Container.GetInstance<IReleaseService>();
        }

        [TestCase("1.0.0.0", "1.0.0.1")]
        public void Bug_Fix_command_increments_minor_version_number_by_one(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.BugFix;

            var release = new Release(previousVersionNumber, (int)releaseType);
            var strategy = _releaseService.GetVersionStrategyForReleaseType(release.ReleaseType);
            var versionNumberStrategyService = new VersionNumberStrategyService(strategy);

            versionNumberStrategyService.IncrementVersionNumber(release);

            var actualVersionNumber = release.VersionNumber;

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.1.11", "1.0.2.0")]
        public void Feature_command_increments_majot_version_number_by_on_and_resets_minor_version_to_zero(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.Feature;

            var release = new Release(previousVersionNumber, (int)releaseType);
            var strategy = _releaseService.GetVersionStrategyForReleaseType(release.ReleaseType);
            var versionNumberStrategyService = new VersionNumberStrategyService(strategy);

            versionNumberStrategyService.IncrementVersionNumber(release);

            var actualVersionNumber = release.VersionNumber;

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.0.0")]
        public void Current_version_number_can_be_read_from_ProductInfo_file(string expectedVersionNumber)
        {
            var actualVersionNumber = _releaseService.ReadVersionNumberFromFile();

            Assert.IsNotNull(actualVersionNumber);
            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.1.2", "1.0.2.0")]
        public void Updated_version_number_can_be_written_to_ProductInfo_file(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.Feature;
            var release = new Release(previousVersionNumber, (int)releaseType);
            var strategy = _releaseService.GetVersionStrategyForReleaseType(release.ReleaseType);
            var versionNumberStrategyService = new VersionNumberStrategyService(strategy);

            versionNumberStrategyService.IncrementVersionNumber(release);

            _releaseService.WriteVersionNumberToFile(release);

            var versionNumberInFile = _releaseService.ReadVersionNumberFromFile();

            Assert.AreNotEqual(previousVersionNumber, versionNumberInFile);
            Assert.AreEqual(expectedVersionNumber, versionNumberInFile);
        }

        [TearDown]
        public void TearDown()
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.Feature;
            var release = new Release("1.0.0.0", (int)releaseType);

            _releaseService.WriteVersionNumberToFile(release);
        }
    }
}
