using Microsoft.AspNetCore.Mvc;
using vizar.Dtos;
using vizar.Entities;
namespace vizar.repositiory
{
    public class FTPImageRepository : IImagesRepository
    {
        public static string RepositioryFolder = "\\Images\\";
        public FTPImageRepository(){
            if (!Directory.Exists(Directory.GetCurrentDirectory() + RepositioryFolder))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + RepositioryFolder);
            }

        }

        public async Task<Guid> UploadImage(IFormFile Image)
        {
            Guid ImageID = Guid.NewGuid();
            var ImagePath = Directory.GetCurrentDirectory() + RepositioryFolder +  ImageID.ToString();

            using (var stream = System.IO.File.Create(ImagePath))
            {
                await Image.CopyToAsync(stream);
                return ImageID;
            }       
        }

        public bool DeleteImage(Guid id)
        {
            var ImagePath = Directory.GetCurrentDirectory() + RepositioryFolder + id.ToString();
            Console.WriteLine(id);
            if(File.Exists(ImagePath)){
                File.Delete(ImagePath);
                return true;
            }
            else
                return false;


        }

        public FileStreamResult GetImage(Guid id)
        {
            var ImagePath = Directory.GetCurrentDirectory() + RepositioryFolder + id.ToString();
            if(File.Exists(ImagePath)){
                var stream = System.IO.File.OpenRead(ImagePath);
                return new FileStreamResult(stream, Utility.GetContentType(".png")); ///application/octet-stream for a Direct Download 
            }
            else
            return null;

        }
    }
}
