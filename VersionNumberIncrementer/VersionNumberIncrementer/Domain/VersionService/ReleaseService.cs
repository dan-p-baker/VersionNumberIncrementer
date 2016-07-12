using System.IO;

namespace VersionNumberIncrementer.Domain.VersionService
{
    public class ReleaseService
    {
        private readonly IVersionNumberStrategy _versionNumberStrategy;

        public ReleaseService(IVersionNumberStrategy versionNumberStrategy)
        {
            _versionNumberStrategy = versionNumberStrategy;
        }

        public void IncrementVersionNumber(Release release)
        {
            _versionNumberStrategy.Increment(release);
        }

        public void WriteVersionNumberToFile(Release release)
        {
            File.WriteAllText("../../ProductInfo.txt", release.VersionNumber);
        }
    }
}