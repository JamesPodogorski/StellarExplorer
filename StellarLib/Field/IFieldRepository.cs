using System;

namespace StellarLib;

public interface IFieldRepository : IRepository<Field>
{
    Task<IEnumerable<Field>> GetAll();
}
