using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record ProductDto{
        [Required]public string Name{get;init;}
        [Required]public float Price{get;init;}
        [Required]public string Description{get;init;}
        [Required]public string Categorie{get;init;}
        [Required]public Guid SellerID{get;init;}
        [Required]public string SellerName{get;init;}
        [Required]public string WebLink{get;init;}
        [Required]public float Width{get;init;}
        [Required]public float Height{get;init;}
        [Required]public float Depth{get;init;}
        [Required]public float Weight{get;init;}
        [Required]public float Volume{get;init;}
        [Required]public Guid ImageID {get;init;}
        [Required]public Guid ModelID{get;init;}
        [Required]public string ModelExtension{get;init;}
    }
}