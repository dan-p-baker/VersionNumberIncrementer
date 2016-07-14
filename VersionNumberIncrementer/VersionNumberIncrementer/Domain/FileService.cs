using System;
using System.IO;

namespace VersionNumberIncrementer.Domain
{
    public class FileService : IFileService
    {
        private static string ProductInfoFileLocation => "../../ProductInfo.txt";
        private static Version DefaultVersion => new Version(1, 0, 0, 0);

        void IFileService.WriteVersionNumberToProductInfoFile(ApplicationVersion applicationVersion)
        {
            File.WriteAllText(ProductInfoFileLocation, applicationVersion.Version.ToString());
        }

        string IFileService.ReadVersionNumberFromProductInfoFile()
        {
            if (!File.Exists(ProductInfoFileLocation))
                CreateFile(DefaultVersion);
        
            return File.ReadAllText(ProductInfoFileLocation).Trim();
        }

        void IFileService.CreateNewProductInfoFile(Version version)
        {
            CreateFile(version);
        }

        void IFileService.DeleteProductInfoFile()
        {
            File.Delete(ProductInfoFileLocation);
        }

        private static void CreateFile(Version version)
        {
            using (var streamWriter = File.CreateText(ProductInfoFileLocation))
            {
                streamWriter.WriteLine(version.ToString());
            }
        }
    }
}