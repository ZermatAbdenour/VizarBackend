using Microsoft.AspNetCore.Mvc;
using vizar.Entities;
using vizar.repositiory;
using vizar.Dtos;
using System.IO;
namespace vizar.Controllers
{
    [ApiController]
    [Route("models")]
    public class ModelsController : ControllerBase
    {
        public readonly IModelsRepository repository;

        public ModelsController(IModelsRepository repo)
        {
            repository = repo;
        }

        [HttpPost]
        public async Task<ActionResult<GUIDDto>> UploadModel(IFormFile Model){

            Guid newModelGUID = await repository.UploadModel(Model);
            GUIDDto ModelDto = new (){
                ID = newModelGUID
            };
            if(newModelGUID != Guid.Empty)
                return ModelDto;
            else
                return BadRequest();
        }

        
        [HttpGet("{ModelID}.{modelExtension}")]
        public FileStreamResult GetModel(Guid ModelID,string modelExtension){
            FileStreamResult Model = repository.GetModel(ModelID);

            return Model;
        }

        [HttpDelete("{ModelID}")]
        public ActionResult DeleteModel(Guid ModelID){
            bool Deleted = repository.DeleteModel(ModelID);
            if(Deleted)
                return Ok();
            else
                return NotFound();
        }
    }
}