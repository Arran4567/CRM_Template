using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Models;
using System;

namespace Bicks.Areas.Booking.Data.DAL
{
    public class BookingWorkUnit : IDisposable
    {
        private ApplicationDbContext _context;
        private GenericRepository<Bookings> bookingRepository;
        private GenericRepository<Client> clientRepository;
        private bool disposed = false;

        public BookingWorkUnit(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Bookings> BookingRepository
        {
            get
            {
                if (bookingRepository == null)
                {
                    bookingRepository = new GenericRepository<Bookings>(_context);
                }
                return bookingRepository;
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

        public void Save()
        {
            _context.SaveChanges();
        }

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
