namespace vizar.Entities{
    public record user{
        public Guid Id{get;init;}
        public string UserName{get;init;}
        public string Email{get;init;}
        public string Password{get;init;}
        public string FullName{get;init;}
        public string Mobile{get;init;}
        public string Adresse{get;init;}
        public List<Guid> UserProducts{get;init;}
        public List<Guid> SavedProducts{get;init;}
        public Guid ImageID{get;init;}
    }
}