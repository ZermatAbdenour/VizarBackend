using Microsoft.AspNetCore.Mvc;
using vizar.Dtos;
using vizar.Entities;
namespace vizar.repositiory
{
    public class FTPModelsRepository : IModelsRepository
    {
        public static string RepositioryFolder = "\\Models\\";
        public FTPModelsRepository(){
            if (!Directory.Exists(Directory.GetCurrentDirectory() + RepositioryFolder))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + RepositioryFolder);
            }

        }

        public async Task<Guid> UploadModel(IFormFile Model)
        {
            Guid ModelID = Guid.NewGuid();
            var ModelPath = Directory.GetCurrentDirectory() + RepositioryFolder +  ModelID.ToString();
            using (var stream = File.Create(ModelPath))
            {
                await Model.CopyToAsync(stream);
                Console.WriteLine(ModelPath);
                return ModelID;
            }           
        }

        public bool DeleteModel(Guid id)
        {
            var ModelPath = Directory.GetCurrentDirectory() + RepositioryFolder + id.ToString();

            if(File.Exists(ModelPath)){
                File.Delete(ModelPath);
                return true;
            }
            else
                return false;
        }

        public FileStreamResult GetModel(Guid id)
        {
            var ModelPath = Directory.GetCurrentDirectory() + RepositioryFolder + id.ToString();
            if(File.Exists(ModelPath)){
                var stream = System.IO.File.OpenRead(ModelPath);
                return new FileStreamResult(stream, "application/model"); ///application/octet-stream for a Direct Download 
            }
            else
            return null;

        }

    }
}
