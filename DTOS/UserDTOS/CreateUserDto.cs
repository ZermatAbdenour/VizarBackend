using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record CreateUserDto{
        [Required]public string Name{get;init;}
        [Required]public string Email{get;init;}
        [Required]public string Password{get;init;}
    }
}