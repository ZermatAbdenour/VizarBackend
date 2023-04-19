using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record UpdatePasswordDto{
        
        [Required]public string OldPassword{get;init;}
        [Required]public string NewPassword{get;init;}
    }
}