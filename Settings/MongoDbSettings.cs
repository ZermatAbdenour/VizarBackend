namespace vizar.Settings{
    public class MongoDbSettings{
        public const string Host = "localhost";
        public const string Port = "27017";

        public string ConnectionString{
            get{
                return $"mongodb://{Host}:{Port}";
            }
        }  
    }

}