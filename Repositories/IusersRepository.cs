using vizar.Entities;

namespace vizar.repositiory
{
    public interface IusersRepository
    {
        IEnumerable<user> GetUsers();
        user GetUser(Guid id);
        user GetUserByEmail(string Email);
        user GetUserByName(string Name);
        void CreateUser(user newuser);
        void UpdateUser(user updateduser);
        void DeleteUser(Guid id);
        List<Product> GetUserProductsList(Guid userid);
        List<Product> GetSavedProductsList(Guid userid);

        void AddUserProduct(Guid userid,Guid productid);
        void RemoveUserProduct(Guid userid,Guid productid);

        void SetProductSavedStat(Guid userid,Guid productid,bool SaveStat);
        bool GetProductSavedStat(Guid userid,Guid productid);
        void RemoveSavedProductFromAllUser(Guid productid);
    }
}

