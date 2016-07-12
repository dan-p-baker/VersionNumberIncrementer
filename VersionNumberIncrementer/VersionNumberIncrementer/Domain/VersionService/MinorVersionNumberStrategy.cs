namespace VersionNumberIncrementer.Domain.VersionService
{
    public class MinorVersionNumberStrategy : IVersionNumberStrategy
    {
        public void Increment(Release release)
        {
            release.MinorVersion ++;
        }
    }
}