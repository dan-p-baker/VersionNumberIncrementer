
using System;

namespace VersionNumberIncrementer.Domain
{
    public interface IFileService
    {
        void WriteVersionNumberToProductInfoFile(ApplicationVersion applicationVersion);
        string ReadVersionNumberFromProductInfoFile();
        void CreateNewProductInfoFile(Version version);
        void DeleteProductInfoFile();
    }
}