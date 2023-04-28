using Microsoft.AspNetCore.Mvc;
using vizar.Entities;
using vizar.repositiory;
using vizar.Dtos;

namespace vizar.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        public readonly IProductsRepository repository;
        public readonly IusersRepository usersrepository;
        public readonly IImagesRepository imagesrepository;
        public readonly IModelsRepository modelsrepository;

        public ProductsController(IProductsRepository repo,IusersRepository usersrepo,IImagesRepository imagesRepo,IModelsRepository modelsrepo)
        {
            repository = repo;
            usersrepository = usersrepo;
            imagesrepository = imagesRepo;
            modelsrepository = modelsrepo;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return repository.GetProducts();
        }

        
        [Route("scroll")]
        [HttpGet]
        public IEnumerable<Product> GetProducts(int Offset,int productsCount)
        {
            return repository.GetProducts(Offset,productsCount);
        }
        [Route("search")]
        [HttpGet]
        public IEnumerable<Product> SearchProducts(string searchquery)
        {
            return repository.SearchProducts(searchquery);
        }

        [HttpGet("search/FullSearch")]
        public IEnumerable<Product> FullSearchProducts(string query,int Offset,int productscount,string categories,float minPrice,float maxPrice)
        {
            return repository.FullSearchProducts(query,Offset,productscount,categories,minPrice,maxPrice);
        }

        [Route("search/autocomplete")]
        [HttpGet]
        public IEnumerable<Product> AutoComplete(string query)
        {
            return repository.AutoComplete(query);
        }
   

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(Guid id){
            var returnproduct =  repository.GetProduct(id);
            if(returnproduct == null)
                return NotFound();
            return returnproduct;
        }


        /*
        [HttpPost()]
        public ActionResult<Product> CreateProduct(ProductDto createDto){
            Product newproduct = new(){
                Id = Guid.NewGuid(),
                PublishDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Name = createDto.Name,
                Price = createDto.Price,
                Description = createDto.Description,
                Categorie = createDto.Categorie,
                SellerID = createDto.SellerID,
                SellerName = createDto.SellerName,
                WebLink = createDto.WebLink,
                Width = createDto.Width,
                Height = createDto.Height,
                Depth = createDto.Depth,
                Weight = createDto.Weight,
                Volume = createDto.Volume,
                ImageID = createDto.ImageID,
                ModelID = createDto.ModelID,
                ModelExtension = createDto.ModelExtension
            };

            //Create Product
            repository.CreateProduct(newproduct);
            
            //add the product to seller products
            usersrepository.AddUserProduct(newproduct.SellerID,newproduct.Id);

            return newproduct;
        }
        */

        [HttpPost()]
        public async Task CreateProduct(IFormFile ImageFile,IFormFile ModelFile,[FromForm]ProductDto createDto){
            Guid modelId = await modelsrepository.UploadModel(ModelFile);
            Guid imageId = await imagesrepository.UploadImage(ImageFile);
            
            Product newproduct = new(){
                Id = Guid.NewGuid(),
                PublishDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Name = createDto.Name,
                Price = createDto.Price,
                Description = createDto.Description,
                Categorie = createDto.Categorie,
                SellerID = createDto.SellerID,
                SellerName = createDto.SellerName,
                WebLink = createDto.WebLink,
                Width = createDto.Width,
                Height = createDto.Height,
                Depth = createDto.Depth,
                Weight = createDto.Weight,
                Volume = createDto.Volume,
                ImageID = imageId,
                ModelID = modelId,
                ModelExtension = createDto.ModelExtension
            };

            //Create Product
            repository.CreateProduct(newproduct);
            
            //add the product to seller products
            usersrepository.AddUserProduct(newproduct.SellerID,newproduct.Id);

        }

        /*
        [HttpPost("")]
        public async Task<ActionResult<Product>> CreateProduct(ProductDto createDto,IFormFile image,IFormFile model){


            Guid imageGuid = await imagesrepository.UploadImage(image);
            
            Guid modelGuid = await modelsrepository.UploadModel(model);

            Product newproduct = new(){
                Id = Guid.NewGuid(),
                PublishDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Name = createDto.Name,
                Price = createDto.Price,
                Description = createDto.Description,
                Categorie = createDto.Categorie,
                SellerID = createDto.SellerID,
                SellerName = createDto.SellerName,
                WebLink = createDto.WebLink,
                Width = createDto.Width,
                Height = createDto.Height,
                Depth = createDto.Depth,
                Weight = createDto.Weight,
                Volume = createDto.Volume,
                ImageID = imageGuid,
                ModelID = modelGuid
            };

            //Create Product
            repository.CreateProduct(newproduct);

            //add the product to seller products
            usersrepository.AddUserProduct(newproduct.SellerID,newproduct.Id);

            return newproduct;
        }
        */

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id,[FromForm]bool ImageUpdated,[FromForm]bool ModelUpdated,IFormFile ImageFile,IFormFile ModelFile,[FromForm]ProductDto updateDto){
            var existingProduct = repository.GetProduct(id);

            if(existingProduct is null)
                return NotFound();

            Guid ImageID = existingProduct.ImageID;
            Guid ModelID = existingProduct.ModelID;
            if(ImageUpdated){
                //replace Image
                imagesrepository.DeleteImage(existingProduct.ImageID);
                ImageID = await imagesrepository.UploadImage(ImageFile);
            }
            if(ModelUpdated){
                //replace Image
                modelsrepository.DeleteModel(existingProduct.ModelID);
                ModelID = await modelsrepository.UploadModel(ModelFile);
            }

            Product UpdatedProduct = existingProduct with{
                Name = updateDto.Name,
                PublishDate = existingProduct.PublishDate,
                Price = updateDto.Price,
                Description = updateDto.Description,
                Categorie = updateDto.Categorie,
                SellerID = updateDto.SellerID,
                SellerName = updateDto.SellerName,
                WebLink = updateDto.WebLink,
                Width = updateDto.Width,
                Height = updateDto.Height,
                Depth = updateDto.Depth,
                Weight = updateDto.Weight,
                Volume = updateDto.Volume,
                ImageID = ImageID,
                ModelID = ModelID,
                ModelExtension = updateDto.ModelExtension
                
            };

            repository.UpdateProduct(UpdatedProduct);

            return Ok();
        }


        [HttpDelete("{id}")]

        public ActionResult DeleteProduct(Guid id){
            var existingProduct = repository.GetProduct(id);

            if(existingProduct is null)
                return NotFound();

            repository.DeleteProduct(id);

            //update the seller 
            usersrepository.RemoveUserProduct(existingProduct.SellerID,id);

            //Delete Product Image
            imagesrepository.DeleteImage(existingProduct.ImageID);
            //Delete Product Model
            modelsrepository.DeleteModel(existingProduct.ModelID);

            usersrepository.RemoveSavedProductFromAllUser(id);
            return NoContent();
        }
    }
}