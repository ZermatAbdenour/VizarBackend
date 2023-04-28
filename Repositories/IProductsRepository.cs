using vizar.Entities;

namespace vizar.repositiory
{
    public interface IProductsRepository
    {
        public static MongoDBProductsRepository Instance{get;private set;}
        //Crud
        Product GetProduct(Guid id);
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProducts(int Offset,int productscount);
        IEnumerable<Product> SearchProducts(string query);
        IEnumerable<Product> FullSearchProducts(string query,int Offset,int productscount,string categories,float minPrice,float maxPrice);
        IEnumerable<Product> AutoComplete(string query);
        void CreateProduct(Product newproduct);
        void UpdateProduct(Product updatedProduct);
        void UpdateProductSellerName(Guid id,string Name);
        void DeleteProduct(Guid id);     


        //Task<IEnumerable<Product>> SearchProduct(String searchquery);
    }
}
