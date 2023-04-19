using Microsoft.AspNetCore.Mvc;
using vizar.Dtos;
using vizar.Entities;

namespace vizar.repositiory
{
    public interface IModelsRepository
    {
        FileStreamResult GetModel(Guid id);
        bool DeleteModel(Guid id);
        Task<Guid> UploadModel(IFormFile Models);
    }
}
