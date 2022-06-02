using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Models;
using Bicks.Areas.Invoicing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Rotativa.AspNetCore;

namespace Bicks.Areas.Invoicing.Data.DAL
{
    public class InvoicingWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<Product> productRepository;

        public InvoicingWorkUnit(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
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

        public SalesInvoiceViewModel GenerateExampleInvoice()
        {
            Product streakyBacon = ProductRepository.GetByID(1);
            streakyBacon.Category = _context.Categories.Find(1);
            Product blackPudding = ProductRepository.GetByID(2);
            blackPudding.Category = _context.Categories.Find(1);
            Product porkLoin = ProductRepository.GetByID(3);
            porkLoin.Category = _context.Categories.Find(2);
            InvoiceItem streakyBaconItem = new InvoiceItem()
            {
                Product = streakyBacon,
                NumCases = 35,
                TotalWeight = 27.24m
            };
            InvoiceItem blackPuddingItem = new InvoiceItem()
            {
                Product = blackPudding,
                NumCases = 35,
                TotalWeight = 10m
            };
            InvoiceItem porkLoinItem = new InvoiceItem()
            {
                Product = porkLoin,
                NumCases = 35,
                TotalWeight = 8.4m
            };
            List<InvoiceItem> invoiceItems = new List<InvoiceItem>
            {
                streakyBaconItem,
                blackPuddingItem,
                porkLoinItem
            };

            decimal total = 0m;
            foreach (InvoiceItem item in invoiceItems)
            {
                total += item.Product.PricePerKg * item.TotalWeight;
            }

            SalesInvoiceViewModel salesInvoiceViewModel = new SalesInvoiceViewModel()
            {
                InvoiceNo = 075741,
                Date = DateTime.Today,
                InvoiceTo = "Moor Farm",
                DeliverTo = "Moor Farm",
                InvoiceItems = invoiceItems,
                Total = total
            };
            return salesInvoiceViewModel;
        }

        //Example
        //public IEnumerable<Job> GetUnassignedJobs()
        //{
        //     return JobRepository.Get(j => j.JobStatus.ID == (int)Enums.JobStatuses.JobCreated, q => q.OrderBy(j => j.DueWhen));
        //}
    }
}
