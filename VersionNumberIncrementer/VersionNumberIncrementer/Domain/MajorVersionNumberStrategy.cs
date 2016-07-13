namespace VersionNumberIncrementer.Domain
{
    public class MajorVersionNumberStrategy : IVersionNumberStrategy
    {
        public void Increment(Release release)
        {
            release.MajorVersion ++;
            release.MinorVersion = 0;
        }
    }
}