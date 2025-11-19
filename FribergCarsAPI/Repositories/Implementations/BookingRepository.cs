using Microsoft.EntityFrameworkCore;
using FribergCars.Data;
using FribergCars.Models;
using FribergCars.Repositories.Interfaces;

namespace FribergCars.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Booking> GetAll()
        {
            return _context.Bookings
                .Include(b => b.Car)
                .Include(b => b.Customer)
                .ToList();
        }
        public Booking GetById(int id) => _context.Bookings.Find(id);
        public void Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }

        public void Delete (int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }

        public List<Booking> GetByCustomerId(int customerId)
        {
            return _context.Bookings
                .Include(b => b.Car)
                .Where(b => b.CustomerId == customerId)
                .ToList();
        }
    }
}
