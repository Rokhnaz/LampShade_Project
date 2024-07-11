using _0_Framework.Application;
using ShopManagement.Domain.ProductPictureAgg;
using Microsoft.AspNetCore.Hosting;


namespace ServiceHost
{
    public class FileUploader : IFileUploder
    {
        private readonly IWebHostEnvironment _environment;
        public FileUploader(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string Upload(IFormFile file,string path)
        {
            if (file == null) return "";

            var directoryPath = $"{_environment.WebRootPath}//ProductPictures//{path}";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = $"{directoryPath}//{file.FileName}";
            using var output = File.Create(filePath);
            file.CopyTo(output);
            return $"{path}/{file.FileName}";
        }
    }
}
