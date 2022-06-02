using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Areas.ClientManagement.Data.DAL
{
    public class ClientManagementWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<Client> clientManagementRepository;

        public ClientManagementWorkUnit(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Client> ClientManagementRepository
        {
            get
            {
                if (clientManagementRepository == null)
                {
                    clientManagementRepository = new GenericRepository<Client>(_context);
                }
                return clientManagementRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
