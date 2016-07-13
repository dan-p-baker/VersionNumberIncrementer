namespace VersionNumberIncrementer.Domain
{
    public class MinorVersionNumberStrategy : IVersionNumberStrategy
    {
        public void Increment(Release release)
        {
            release.MinorVersion ++;
        }
    }
}