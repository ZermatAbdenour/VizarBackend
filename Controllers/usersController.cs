using Microsoft.AspNetCore.Mvc;
using vizar.Entities;
using vizar.repositiory;
using vizar.Dtos;
using BCrypt.Net;

namespace vizar.Controllers
{
    [ApiController]
    [Route("users")]
    public class usersController : ControllerBase
    {
        public readonly IusersRepository repository;
        public readonly IProductsRepository productsRepository;
        public readonly IImagesRepository imagesRepository;

        public usersController(IusersRepository repo,IProductsRepository productsRepo,IImagesRepository imagesrepo)
        {
            repository = repo;
            productsRepository = productsRepo;
            imagesRepository = imagesrepo;
        }

        [HttpGet]
        public IEnumerable<user> GetUsers()
        {
            return repository.GetUsers();
        }

        [HttpGet("{id}")]
        public ActionResult<user> GetUser(Guid id){
            var returnuser =  repository.GetUser(id);
            if(returnuser == null)
                return NotFound();
            return returnuser;
        }

        [Route("register")]
        [HttpPost()]
        public async Task<ActionResult<user>> CreateUser(CreateUserDto createDto){
            user CheckEmail = repository.GetUserByEmail(createDto.Email);
            if(CheckEmail != null)
                return Unauthorized();
            user CheckName = repository.GetUserByName(createDto.Name);
            if(CheckName != null)
                return Unauthorized();

            



            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(createDto.Password, BCrypt.Net.BCrypt.GenerateSalt());

            user newuser = new(){
                Id = Guid.NewGuid(),
                Verified = false,
                UserName = createDto.Name,
                Email = createDto.Email,
                Password = hashedPassword,
                FullName = String.Empty,
                Mobile = String.Empty,
                Adresse = String.Empty,
                UserProducts = new List<Guid>(),
                SavedProducts = new List<Guid>()

            };

            

 
            //Send email confirmation
            MailRequest request = new (){
                ToEmail = createDto.Email,
                Userid = newuser.Id.ToString()
            };
            
            bool sendEmail = await repository.SendEmailAsync(request);
            if(!sendEmail)
                return NotFound();

            repository.CreateUser(newuser);
            
            return newuser;
        }
        [Route("login")]
        [HttpPost()]
        public ActionResult<user> Login(LoginDto loginDto){
            var user = repository.GetUserByEmail(loginDto.Email);
            if(user == null)
                return NotFound();
            if(!user.Verified)
                return Forbid();
            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return Unauthorized();
            else return user;
        }
        [Route("update/password/{id}")]
        [HttpPut()]
        public ActionResult<user> UpdatePassword(Guid id,UpdatePasswordDto updatePassWorddto){
            user User = repository.GetUser(id);
            if(User == null)
                return NotFound();
            
            if(!BCrypt.Net.BCrypt.Verify(updatePassWorddto.OldPassword, User.Password))
                return Unauthorized();
            
            //Update password

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatePassWorddto.NewPassword, BCrypt.Net.BCrypt.GenerateSalt());

            user updatedUser = User with{
                Password = hashedPassword,
            };

            repository.UpdateUser(updatedUser);
            return updatedUser;
        }
        [Route("update/emailverification/{id}")]
        [HttpGet()]
        public ActionResult<user> VerifyEmail(Guid id){

            if(repository.VerifyUser(id))
                return Ok();
            
            return NotFound();
        }

        [Route("update/profile/{id}")]
        [HttpPut()]
        public ActionResult<user> UpdateUser(Guid id,UpdateProfileDto updateDto){
            var existingUser = repository.GetUser(id);

            if(existingUser is null)
                return NotFound();

            user UpdatedUser = existingUser with{
                UserName = updateDto.UserName,
                FullName = updateDto.FullName,
                Mobile = updateDto.Mobile,
                Adresse = updateDto.Adresse,
            };

            repository.UpdateUser(UpdatedUser);

            //Update the Seller Name on the User Products

            for(int i =0;i<UpdatedUser.UserProducts.Count;i++){
                productsRepository.UpdateProductSellerName(UpdatedUser.UserProducts[i],updateDto.UserName);
            }

            return UpdatedUser;
        }
        [Route("update/image/{id}")]
        [HttpPut()]
        public async Task<ActionResult<user>> SetProfileImage(Guid id,IFormFile Image){
            var User = repository.GetUser(id);

            if(User is null)
                return NotFound();

            Guid newImageGUID = Guid.Empty;
            if(User.ImageID != Guid.Empty)
                imagesRepository.DeleteImage(User.ImageID);


            newImageGUID = await imagesRepository.UploadImage(Image);

            if(newImageGUID == Guid.Empty)
                return BadRequest();
            
            //Update Image id in the users repository
            user UpdatedUser = User with{
                ImageID = newImageGUID
            };

            repository.UpdateUser(UpdatedUser);

            return UpdatedUser;
        }

        [HttpGet("seller/{id}")]
        public ActionResult<SellerDto> GetSellerByID(Guid id){
            var user = repository.GetUser(id);
            if(user == null)
                return NotFound();
            SellerDto seller = new (){
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Mobile = user.Mobile,
                Adresse = user.Adresse,
                ImageID = user.ImageID
            };

            return seller;
        }

 

        [HttpDelete("{id}")]

        public ActionResult DeleteUser(Guid id){
            var existingUser = repository.GetUser(id);

            if(existingUser is null)
                return NotFound();

            repository.DeleteUser(id);

            return NoContent();
        }

        

        //User Products
        [HttpGet("userproducts/{id}")]
        public ActionResult<List<Product>> GetUserProducts(Guid id){
            return repository.GetUserProductsList(id);
        }

        [HttpGet("savedproducts/{id}")]
        public ActionResult<List<Product>> GetSavedProducts(Guid id){
            return repository.GetSavedProductsList(id);
        }

        [HttpPost("savedproducts/stat/{id}")]
        public void SetProductSavedStat(Guid id,Guid productid,bool savestat){
            repository.SetProductSavedStat(id,productid,savestat);
        }

        [HttpGet("savedproducts/stat/{id}")]
        public bool SetProductSavedStat(Guid id,Guid productid){
            return repository.GetProductSavedStat(id,productid);
        }

        [HttpGet("sendEmail")]
        public async Task<ActionResult> Sendmil(string Email,Guid id){
            //Send email confirmation
            MailRequest request = new (){
                ToEmail = Email,
                Userid = id.ToString()

            };
            await repository.SendEmailAsync(request);
            return Ok();
        }

    }
}