using CardStorageService.Data;
using CardStorageService.Data.Entity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CardStorageService.Services.Impl
{
    public class ClientRepository : IClientRepositoryService
    {
        
        #region Services

        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        #endregion

        #region Constructors

        public ClientRepository(
            ILogger<ClientRepository> logger,
            CardStorageServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        public int Create(Client data)
        {
            _context.Clients.Add(data);
            _context.SaveChanges();
            return data.ClientId;
        }

        public int Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Client> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Client GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public int Update(Client data)
        {
            throw new System.NotImplementedException();
        }
    }
}
