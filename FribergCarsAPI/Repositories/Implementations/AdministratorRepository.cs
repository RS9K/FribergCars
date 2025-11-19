using Microsoft.EntityFrameworkCore;
using FribergCars.Data;
using FribergCars.Models;
using FribergCars.Repositories.Interfaces;

namespace FribergCars.Repositories.Implementations
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly AppDbContext _context;

        public AdministratorRepository(AppDbContext context)
        {
            _context = context;
        }

        public Administrator GetByEmailAndPassword(string email, string password)
        {
            return _context.Administrators
            .FirstOrDefault(a => a.Email == email && a.Password == password);
        }

        public List<Administrator> GetAll() => _context.Administrators.ToList();
        public Administrator GetById(int id) => _context.Administrators.Find(id);
        public void Add (Administrator administrator)
        {
            _context.Administrators.Add(administrator);
            _context.SaveChanges();
        }
        public void Update(Administrator administrator)
        {
            _context.Administrators.Update(administrator);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var administrator = _context.Administrators.Find(id);
            if (administrator != null)
            {
                _context.Administrators.Remove(administrator);
                _context.SaveChanges();
            }
        }
    }
}
