namespace VersionNumberIncrementer.Domain.VersionService
{
    public class MajorVersionNumberStrategy : IVersionNumberStrategy
    {
        public void Increment(Release release)
        {
            release.MajorVersion ++;
        }
    }
}