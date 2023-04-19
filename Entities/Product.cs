namespace vizar.Entities{
    public record Product{
        public Guid Id{get;init;}
        public string PublishDate{get;init;}
        public string Name{get;init;}
        public float Price{get;init;}
        public string Description{get;init;}
        public string Categorie{get;init;}
        
        public Guid SellerID{get;init;}
        public string SellerName{get;init;}
        public string WebLink{get;init;}
        public float Width{get;init;}
        public float Height{get;init;}
        public float Depth{get;init;}
        public float Weight{get;init;}
        public float Volume{get;init;}
        public Guid ImageID{get;init;}
        public Guid ModelID{get;init;}
        public string ModelExtension{get;init;}
    }
}