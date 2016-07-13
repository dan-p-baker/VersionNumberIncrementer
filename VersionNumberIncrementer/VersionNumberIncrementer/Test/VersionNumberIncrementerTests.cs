using System;
using NUnit.Framework;
using VersionNumberIncrementer.Domain;

namespace VersionNumberIncrementer.Test
{
    public class VersionNumberIncrementerTests
    {
        private static IFileService _fileService;

        [SetUp]
        public void Setup()
        {
            Bootstrap.Start();
            _fileService = Bootstrap.Container.GetInstance<IFileService>();
        }

        [TestCase("1.0.0.0", "1.0.0.1")]
        public void Bug_Fix_command_increments_minor_version_number_by_one(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.BugFix;

            var release = new Release(previousVersionNumber);
            release = release.IncrementVersion(releaseType);

            var actualVersionNumber = release.Version.ToString();

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.1.11", "1.0.2.0")]
        public void Feature_command_increments_major_version_number_by_on_and_resets_minor_version_to_zero(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.Feature;

            var release = new Release(previousVersionNumber);
            release = release.IncrementVersion(releaseType);

            var actualVersionNumber = release.Version.ToString();

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.0.0")]
        public void Current_version_number_can_be_read_from_ProductInfo_file(string expectedVersionNumber)
        {
            var actualVersionNumber = _fileService.ReadVersionNumberFromFile();

            Assert.IsNotNull(actualVersionNumber);
            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.1.2", "1.0.2.0")]
        public void Updated_version_number_can_be_written_to_ProductInfo_file(string previousVersionNumber, string expectedVersionNumber)
        {
            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.Feature;

            var release = new Release(previousVersionNumber);
            release = release.IncrementVersion(releaseType);

            _fileService.WriteVersionNumberToFile(release);

            var versionNumberInFile = _fileService.ReadVersionNumberFromFile();

            Assert.AreNotEqual(previousVersionNumber, versionNumberInFile);
            Assert.AreEqual(expectedVersionNumber, versionNumberInFile);
        }

        [TestCase("1.0.1.asdf2")]
        public void Invalid_version_number_throws_format_exception(string invalidVersionNumber)
        {
            Assert.Throws<FormatException>(() =>
            {
                var release = new Release(invalidVersionNumber);
            });
        }

        [Test]
        public void Attempting_to_increment_version_number_past_max_int_throws_overflow_exception()
        {
            const int maxInt = int.MaxValue;
            var versionNumberAtMaxInt = $"1.0.0.{maxInt}";

            const Release.ReleaseTypeEnum releaseType = Release.ReleaseTypeEnum.BugFix;

            var release = new Release(versionNumberAtMaxInt);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                release = release.IncrementVersion(releaseType);
            });
        }

        [TearDown]
        public void TearDown()
        {
            var release = new Release("1.0.0.0");
            _fileService.WriteVersionNumberToFile(release);
        }
    }
}
