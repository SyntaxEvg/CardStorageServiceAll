using CardStorageService.Data;
using CardStorageService.Data.Entity;
using System.Collections.Generic;

namespace CardStorageService.Services
{
    public interface ICardRepositoryService : IRepository<Card, string>
    {
        IList<Card> GetByClientId(string id);
    }
}
