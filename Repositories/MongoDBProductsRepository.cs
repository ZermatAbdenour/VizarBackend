using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Labs.Search;
using vizar.Entities;

namespace vizar.repositiory
{
    public class MongoDBProductsRepository : IProductsRepository
    {


        private const string CollectionName = "Products";
        private const string DatabaseName = "Vizar";
        private readonly IMongoCollection<Product> ProductsCollection;
        private readonly FilterDefinitionBuilder<Product> filterBuilder = Builders<Product>.Filter;
        public MongoDBProductsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
            ProductsCollection = database.GetCollection<Product>(CollectionName);

        }



        public void CreateProduct(Product newproduct)
        {
            ProductsCollection.InsertOne(newproduct);
        }

        public void DeleteProduct(Guid Id)
        {
            var filter = filterBuilder.Eq(Product => Product.Id, Id);
            ProductsCollection.DeleteOne(filter);
        }

        public Product GetProduct(Guid Id)
        {
            var filter = filterBuilder.Eq(Product => Product.Id, Id);
            return ProductsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            var Products = ProductsCollection.Find(new BsonDocument()).ToEnumerable();
            return Products;

        }


        public IEnumerable<Product> GetProducts(int Offset, int productscount)
        {
            var Products = ProductsCollection.Find(new BsonDocument()).Skip(Offset).Limit(productscount).ToList();

            return Products;
        }
        /*
        public IEnumerable<Product> SearchProducts(string query)
        {
            var pipeline = new BsonDocument[]
            {
            new BsonDocument("$search",
            new BsonDocument
            {
                { "index", "SearchProducts" },
                { "text",
                new BsonDocument
                {
                    { "query", query },
                    { "path",
                new BsonDocument("wildcard", "*") }
            } }
        })
            };
            var result = ProductsCollection.Aggregate<Product>(pipeline).ToList();

            return result;
        }
        */
        public IEnumerable<Product> SearchProducts(string query)
        {

            var result = ProductsCollection.Aggregate().Search( SearchBuilders<Product>.Search
                    .Text(query, x => x.Name)).ToList();

            return result;
        }
        public IEnumerable<Product> FullSearchProducts(string query,int Offset,int productscount,string categories,float minPrice,float maxPrice){
            List<Product> result = new List<Product>();

            
            if(categories.Contains("All categories")){
                Console.WriteLine(query);
                Console.WriteLine(!string.IsNullOrWhiteSpace(query));
                if(!string.IsNullOrWhiteSpace(query)){
                    //Search with the query
                    result = ProductsCollection.Aggregate().Search( SearchBuilders<Product>.Search
                        .Compound().Must(SearchBuilders<Product>.Search.Text(query, x => x.Name),SearchBuilders<Product>.Search.RangeDouble(x => x.Price).Gte(minPrice).Lte(maxPrice))).Skip(Offset).Limit(productscount).ToList();
                }else{
                    //Filter products only
                    result = GetProducts(Offset,productscount).ToList();
                }

            }
            else{
                if(!string.IsNullOrWhiteSpace(query)){
                    result = ProductsCollection.Aggregate().Search( SearchBuilders<Product>.Search
                        .Compound().Must(SearchBuilders<Product>.Search
                        .Text(query, x => x.Name),SearchBuilders<Product>.Search.QueryString(x => x.Categorie,GenerateFiltersQueryString(categories)),SearchBuilders<Product>.Search.RangeDouble(x => x.Price).Gte(minPrice).Lte(maxPrice))).Skip(Offset).Limit(productscount).ToList();
                }
                else
                    //
                    result = ProductsCollection.Aggregate().Search( SearchBuilders<Product>.Search
                        .Compound().Must(SearchBuilders<Product>.Search.QueryString(x => x.Categorie,GenerateFiltersQueryString(categories)),SearchBuilders<Product>.Search.RangeDouble(x => x.Price).Gte(minPrice).Lte(maxPrice))).Skip(Offset).Limit(productscount).ToList();
            }
            



            return result;
        }

        private string GenerateFiltersQueryString(string categories){
            
            string QueryString = "nothing";

            if(categories.Replace(" ","").LastOrDefault() != '|')
                QueryString = categories.Replace("|" ,"OR");

            return QueryString;
        }



        public IEnumerable<Product> AutoComplete(string query)
        {
            var result = ProductsCollection.Aggregate().Search( SearchBuilders<Product>.Search
                    .Autocomplete(query, x => x.Name)).ToList();
            return result;
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var Filter = filterBuilder.Eq(user => user.Id, updatedProduct.Id);
            ProductsCollection.ReplaceOne(Filter, updatedProduct);
        }

        public void UpdateProductSellerName(Guid id, string Name)
        {
            Product product = GetProduct(id);
            Product UpdatedProduct = product with
            {
                SellerName = Name,
            };

            UpdateProduct(UpdatedProduct);
        }
    }
}