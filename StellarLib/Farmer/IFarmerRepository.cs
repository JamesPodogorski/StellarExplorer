using System;

namespace StellarLib;

public interface IFarmerRepository : IRepository<Field>
{
    public Task<IEnumerable<Field>> GetAllWithoutCall();
}