namespace VersionNumberIncrementer.Domain
{
    public interface IVersionNumberStrategy
    {
        void Increment(Release release);
    }
}
