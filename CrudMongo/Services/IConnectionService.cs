using MongoDB.Driver;
namespace CrudMongo.Services
{
    public interface IConnectionService
    {
        IMongoDatabase db();
    }
}
