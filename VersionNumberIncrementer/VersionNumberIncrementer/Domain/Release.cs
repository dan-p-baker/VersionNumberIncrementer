using System;

namespace VersionNumberIncrementer.Domain
{
    public class Release
    {
        public enum ReleaseTypeEnum
        {
            Feature,
            BugFix
        }
        protected Version _version;

        public Version Version => _version;

        public Release(string versionNumber)
        {
            _version = ParseVersion(versionNumber);
        }

        private static Version ParseVersion(string input)
        {
            Version version;

            if (Version.TryParse(input, out version))
                return version;

            throw new FormatException("Invalid version number entered.");
        }

        public Release IncrementVersion(ReleaseTypeEnum releaseType)
        {
            Version newVersionNumber;
            switch (releaseType)
            {
                case ReleaseTypeEnum.BugFix:
                    newVersionNumber = new Version(Version.Major, Version.Minor, Version.Build, Version.Revision + 1);
                    return new Release(newVersionNumber.ToString());
                case ReleaseTypeEnum.Feature:
                    newVersionNumber = new Version(Version.Major, Version.Minor, Version.Build + 1, 0);
                    return new Release(newVersionNumber.ToString());
                default:
                    throw new ArgumentOutOfRangeException(nameof(releaseType), releaseType,
                        "Unknown release type entered.");
            }
        }
    }
}