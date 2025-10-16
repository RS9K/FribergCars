using System.Collections.Generic;
using FribergCars.Models;
using FribergCars.Data;

namespace FribergCars.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        List<Booking> GetAll();
        List<Booking> GetByCustomerId(int customerId);
        Booking GetById(int id);
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(int id);
    }
}
