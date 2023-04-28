using Microsoft.AspNetCore.Mvc;
using vizar.Entities;
using vizar.repositiory;
using vizar.Dtos;
using System.IO;

namespace vizar.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        public readonly IImagesRepository repository;

        public ImagesController(IImagesRepository repo)
        {
            repository = repo;
        }

        [HttpPost]
        public async Task<ActionResult<GUIDDto>> UploadImage(IFormFile Image){

            Guid newImageGUID = await repository.UploadImage(Image);
            GUIDDto imageDto = new (){
                ID = newImageGUID
            };
            if(newImageGUID != Guid.Empty)
                return imageDto;
            else
                return BadRequest();
        }

        
        [HttpGet("{imageID}")]
        public FileStreamResult GetImage(Guid imageID){
            FileStreamResult image = repository.GetImage(imageID);

            return image;
        }

        [HttpDelete("{imageID}")]
        public ActionResult DeleteImage(Guid imageID){
            bool Deleted = repository.DeleteImage(imageID);
            if(Deleted)
                return Ok();
            else
                return NotFound();
        }
    }
}