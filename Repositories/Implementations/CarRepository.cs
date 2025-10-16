using Microsoft.EntityFrameworkCore;
using FribergCars.Models;
using FribergCars.Data;
using FribergCars.Repositories.Interfaces;

namespace FribergCars.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;

        public CarRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Car> GetAll() => _context.Cars.ToList();
        public Car GetById(int id) => _context.Cars.Find(id);
        public void Add(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void Update(Car car)
        {
            _context.Cars.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
    }

}
