using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Rotativa.AspNetCore;

namespace Bicks.Areas.Sales.Data.DAL
{
    public class SalesWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<Sale> saleRepository;
        private GenericRepository<Client> clientRepository;
        private GenericRepository<InvoiceItem> invoiceItemRepository;

        public SalesWorkUnit(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Sale> SaleRepository
        {
            get
            {
                if (saleRepository == null)
                {
                    saleRepository = new GenericRepository<Sale>(_context);
                }
                return saleRepository;
            }
        }

        public GenericRepository<Client> ClientRepository
        {
            get
            {
                if (clientRepository == null)
                {
                    clientRepository = new GenericRepository<Client>(_context);
                }
                return clientRepository;
            }
        }

        public GenericRepository<InvoiceItem> InvoiceItemRepository
        {
            get
            {
                if (invoiceItemRepository == null)
                {
                    invoiceItemRepository = new GenericRepository<InvoiceItem>(_context);
                }
                return invoiceItemRepository;
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

        //Example
        //public IEnumerable<Job> GetUnassignedJobs()
        //{
        //     return JobRepository.Get(j => j.JobStatus.ID == (int)Enums.JobStatuses.JobCreated, q => q.OrderBy(j => j.DueWhen));
        //}
    }
}
