using System.Collections.Generic;
using FribergCars.Models;
using FribergCars.Data;

namespace FribergCars.Repositories.Interfaces
{
    public interface IAdministratorRepository
    {
        List<Administrator> GetAll();
        Administrator GetById(int id);
        Administrator GetByEmailAndPassword(string email, string password);
        void Add(Administrator administrator);
        void Update(Administrator administrator);
        void Delete(int id);
    }
}
