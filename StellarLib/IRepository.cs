using System;

namespace StellarLib;

public interface IRepository<T>
{
    Task<T> GetById(string id);
    Task<IEnumerable<T>> GetAll();
    // T Create(T entity);
    // T Update(T entity);
    // bool Delete(string id);
}