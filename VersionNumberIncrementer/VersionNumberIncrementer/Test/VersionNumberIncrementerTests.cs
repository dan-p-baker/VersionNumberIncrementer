using System;
using NUnit.Framework;
using VersionNumberIncrementer.Domain;

namespace VersionNumberIncrementer.Test
{
    public class VersionNumberIncrementerTests
    {
        private static IFileService _fileService;
        private static Version DefaultVersion => new Version(1, 0, 0, 0);

        [SetUp]
        public void Setup()
        {
            Bootstrap.Start();
            _fileService = Bootstrap.Container.GetInstance<IFileService>();
            _fileService.CreateNewProductInfoFile(DefaultVersion);
        }

        [TestCase("1.0.0.0", "1.0.0.1")]
        public void Bug_Fix_command_increments_minor_version_number_by_one(string previousVersionNumber, string expectedVersionNumber)
        {
            const ApplicationVersion.ReleaseTypeEnum releaseType = ApplicationVersion.ReleaseTypeEnum.BugFix;

            var applicationVersion = new ApplicationVersion(previousVersionNumber);
            applicationVersion = applicationVersion.IncrementVersion(releaseType);

            var actualVersionNumber = applicationVersion.ToString();

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.1.11", "1.0.2.0")]
        public void Feature_command_increments_major_version_number_by_one_and_resets_minor_version_to_zero(string previousVersionNumber, string expectedVersionNumber)
        {
            const ApplicationVersion.ReleaseTypeEnum releaseType = ApplicationVersion.ReleaseTypeEnum.Feature;

            var applicationVersion = new ApplicationVersion(previousVersionNumber);
            applicationVersion = applicationVersion.IncrementVersion(releaseType);

            var actualVersionNumber = applicationVersion.ToString();

            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.0.0")]
        public void Current_version_number_can_be_read_from_ProductInfo_file(string expectedVersionNumber)
        {
            var actualVersionNumber = _fileService.ReadVersionNumberFromProductInfoFile();

            Assert.IsNotNull(actualVersionNumber);
            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }

        [TestCase("1.0.0.0")]
        public void Running_application_without_product_info_file_creates_a_new_one_with_default_version_number(string expectedVersionNumber)
        {
            _fileService.DeleteProductInfoFile();

            var actualVersionNumber = _fileService.ReadVersionNumberFromProductInfoFile();

            Assert.IsNotNull(actualVersionNumber);
            Assert.AreEqual(expectedVersionNumber, actualVersionNumber);
        }


        [TestCase("1.0.1.2", "1.0.2.0")]
        public void Updated_version_number_can_be_written_to_ProductInfo_file(string previousVersionNumber, string expectedVersionNumber)
        {
            const ApplicationVersion.ReleaseTypeEnum releaseType = ApplicationVersion.ReleaseTypeEnum.Feature;

            var applicationVersion = new ApplicationVersion(previousVersionNumber);
            applicationVersion = applicationVersion.IncrementVersion(releaseType);

            _fileService.WriteVersionNumberToProductInfoFile(applicationVersion);

            var versionNumberInFile = _fileService.ReadVersionNumberFromProductInfoFile();

            Assert.AreNotEqual(previousVersionNumber, versionNumberInFile);
            Assert.AreEqual(expectedVersionNumber, versionNumberInFile);
        }

        [TestCase("1.0.1.asdf2", "Invalid version number entered.")]
        public void Invalid_version_number_throws_FormatException(string invalidVersionNumber, string expectedErrorMessage)
        {
            var actualException = Assert.Throws<FormatException>(() =>
            {
                var applicationVersion = new ApplicationVersion(invalidVersionNumber);
            });

            Assert.AreEqual(expectedErrorMessage, actualException.Message);
        }

        [TestCase("Cannot increment version number for release as value would be out of range")]
        public void Attempting_to_increment_minor_version_number_past_max_int_throws_InvalidOperationException(string expectedErrorMessage)
        {
            const int maxInt = int.MaxValue;
            var versionNumberAtMaxInt = $"1.0.0.{maxInt}";

            const ApplicationVersion.ReleaseTypeEnum releaseType = ApplicationVersion.ReleaseTypeEnum.BugFix;

            var applicationVersion = new ApplicationVersion(versionNumberAtMaxInt);

            var actualException = Assert.Throws<InvalidOperationException>(() =>
            {
                applicationVersion = applicationVersion.IncrementVersion(releaseType);
            });

            Assert.AreEqual(expectedErrorMessage, actualException.Message);
        }

        [TestCase("Cannot increment version number for release as value would be out of range")]
        public void Attempting_to_increment_major_version_number_past_max_int_throws_InvalidOperationException(string expectedErrorMessage)
        {
            const int maxInt = int.MaxValue;
            var versionNumberAtMaxInt = $"1.0.0.{maxInt}";

            const ApplicationVersion.ReleaseTypeEnum releaseType = ApplicationVersion.ReleaseTypeEnum.BugFix;

            var applicationVersion = new ApplicationVersion(versionNumberAtMaxInt);

            var actualException = Assert.Throws<InvalidOperationException>(() =>
            {
                applicationVersion = applicationVersion.IncrementVersion(releaseType);
            });

            Assert.AreEqual(expectedErrorMessage, actualException.Message);
        }

        [TearDown]
        public void TearDown()
        {
            _fileService.DeleteProductInfoFile();
        }
    }
}
