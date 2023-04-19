using MongoDB.Bson;
using MongoDB.Driver;
using vizar.Entities;

namespace vizar.repositiory{
    public class MongoDBUsersRepository : IusersRepository
    {
        private const string CollectionName = "Users";
        private const string DatabaseName = "Vizar";
        private readonly IMongoCollection<user> UsersCollection;
        private readonly IProductsRepository productsRepository;
        private readonly FilterDefinitionBuilder<user> filterBuilder = Builders<user>.Filter;

        public MongoDBUsersRepository(IMongoClient mongoClient,IProductsRepository productsrepo){
            IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
            UsersCollection = database.GetCollection<user>(CollectionName);

            productsRepository = productsrepo;

        }
        

        public void CreateUser(user newuser)
        {
            UsersCollection.InsertOne(newuser);
        }

        public void DeleteUser(Guid id)
        {
            var Filter = filterBuilder.Eq(user => user.Id, id);
            UsersCollection.DeleteOne(Filter);
        }

        public user GetUser(Guid id)
        {
            var Filter = filterBuilder.Eq(user => user.Id, id);
            return UsersCollection.Find(Filter).SingleOrDefault();
        }

        public IEnumerable<user> GetUsers()
        {
            return UsersCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdateUser(user updateduser)
        {
            var Filter = filterBuilder.Eq(user => user.Id, updateduser.Id);
            UsersCollection.ReplaceOne(Filter,updateduser);
        }

        public user GetUserByEmail(string Email){
            var Filter = filterBuilder.Eq(user => user.Email,Email);
            return UsersCollection.Find(Filter).SingleOrDefault();
        }
        public user GetUserByName(string Name){
            var Filter = filterBuilder.Eq(user => user.UserName,Name);
            return UsersCollection.Find(Filter).SingleOrDefault();
        }



        public List<Product> GetUserProductsList(Guid userid){
            user User = GetUser(userid);
            if(User != null){
                List<Product> UserProducts = new List<Product>();
                foreach(Guid product in User.UserProducts){
                UserProducts.Add(productsRepository.GetProduct(product));
                }

                return UserProducts;
            }
            else
                return null;
        }

        public List<Product> GetSavedProductsList(Guid userid){
            user User = GetUser(userid);
            if(User!=null){
                List<Product> SavedProducts = new List<Product>();

                for(int i =0;i<User.SavedProducts.Count;i++){
                    Product Savedproduct = productsRepository.GetProduct(User.SavedProducts[i]);
                    if(Savedproduct != null)
                        SavedProducts.Add(Savedproduct);
                }

                return SavedProducts;  
            }
            else
                return new List<Product>();
        }

        public void AddUserProduct(Guid userid,Guid productid){
            user User = GetUser(userid);
            if(User != null){
                User.UserProducts.Add(productid);
                UpdateUser(User);
            }
        }

        public void RemoveUserProduct(Guid userid,Guid productid){
            user User = GetUser(userid);
            if(User != null){
                User.UserProducts.Remove(productid);
                UpdateUser(User);
            }
        }

        public void SetProductSavedStat(Guid userid,Guid productid,bool SaveStat){
            user User = GetUser(userid);
            if(User != null){
                if(SaveStat){
                    if(!User.SavedProducts.Contains(productid))
                        User.SavedProducts.Add(productid);
                }
                else
                    User.SavedProducts.Remove(productid);

                UpdateUser(User);
            }
        }

        public bool GetProductSavedStat(Guid userid,Guid productid){
            user User = GetUser(userid);

            if(User.SavedProducts.Contains(productid))
                return true;
            else
                return false;
        }

        public void RemoveSavedProductFromAllUser(Guid productid){
            ///
            /// Remove a product from every user that have it in his saved products
            ///
        }
    }
}