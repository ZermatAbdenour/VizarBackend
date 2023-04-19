using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record GUIDDto{
        [Required]public Guid ID{get;init;}
    }
}