
namespace VersionNumberIncrementer.Domain
{
    public interface IFileService
    {
        void WriteVersionNumberToFile(Release release);
        string ReadVersionNumberFromFile();
    }
}