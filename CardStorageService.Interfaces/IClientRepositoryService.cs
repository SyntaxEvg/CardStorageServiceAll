using CardStorageService.Data;
using CardStorageService.Data.Entity;

namespace CardStorageService.Services
{
    public interface IClientRepositoryService : IRepository<Client, int>
    {
    }
}
