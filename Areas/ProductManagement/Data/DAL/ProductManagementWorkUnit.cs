using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Models;
using Bicks.Areas.Invoicing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Rotativa.AspNetCore;

namespace Bicks.Areas.ProductManagement.Data.DAL
{
    public class ProductManagementWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Category> categoryRepository;

        public ProductManagementWorkUnit(ApplicationDbContext context)
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

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(_context);
                }
                return categoryRepository;
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
