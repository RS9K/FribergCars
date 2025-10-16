using System.Collections.Generic;
using FribergCars.Models;
using FribergCars.Data;

namespace FribergCars.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Customer GetByEmail(string email);
        Customer GetByEmailAndPassword(string email, string password);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
    }
}
