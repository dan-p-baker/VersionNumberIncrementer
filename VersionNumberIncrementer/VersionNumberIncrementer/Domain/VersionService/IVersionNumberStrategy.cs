namespace VersionNumberIncrementer.Domain.VersionService
{
    public interface IVersionNumberStrategy
    {
        void Increment(Release release);
    }
}
