using System;
using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record SellerDto{
        [Required]public string UserName{get;init;}
        [Required]public string FullName{get;init;}
        [Required]public string Email{get;init;}
        [Required]public string Mobile{get;init;}
        [Required]public string Adresse{get;init;}
        [Required]public Guid ImageID{get;init;}
    }
}