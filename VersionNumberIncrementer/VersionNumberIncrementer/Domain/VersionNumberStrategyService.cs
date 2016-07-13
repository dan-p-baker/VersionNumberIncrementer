
namespace VersionNumberIncrementer.Domain
{
    public class VersionNumberStrategyService
    {
        private readonly IVersionNumberStrategy _versionNumberStrategy;

        public VersionNumberStrategyService(IVersionNumberStrategy versionNumberStrategy)
        {
            _versionNumberStrategy = versionNumberStrategy;
        }

        public void IncrementVersionNumber(Release release)
        {
            _versionNumberStrategy.Increment(release);
        }
    }
}