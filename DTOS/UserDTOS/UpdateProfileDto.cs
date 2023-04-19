using System.ComponentModel.DataAnnotations;

namespace vizar.Dtos{
    public record UpdateProfileDto{
        public string UserName{get;init;}
        public string FullName{get;init;}
        public string Mobile{get;init;}
        public string Adresse{get;init;}
    }
}