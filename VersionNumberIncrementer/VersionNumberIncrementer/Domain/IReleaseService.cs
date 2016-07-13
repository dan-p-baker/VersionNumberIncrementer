
namespace VersionNumberIncrementer.Domain
{
    public interface IReleaseService
    {
        void WriteVersionNumberToFile(Release release);
        string ReadVersionNumberFromFile();
        IVersionNumberStrategy GetVersionStrategyForReleaseType(Release.ReleaseTypeEnum releaseType);
    }
}