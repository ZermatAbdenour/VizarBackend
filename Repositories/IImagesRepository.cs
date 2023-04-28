using Microsoft.AspNetCore.Mvc;
using vizar.Dtos;
using vizar.Entities;

namespace vizar.repositiory
{
    public interface IImagesRepository
    {
        FileStreamResult GetImage(Guid id);
        bool DeleteImage(Guid id);
        Task<Guid> UploadImage(IFormFile Image);
    }
}
