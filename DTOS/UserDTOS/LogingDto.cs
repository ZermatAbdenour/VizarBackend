using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record LoginDto{
        [Required]public string Email{get;init;}
        [Required]public string Password{get;init;}
    }
}