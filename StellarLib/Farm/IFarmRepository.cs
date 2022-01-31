using System;

namespace StellarLib;

public interface IFarmRepository : IRepository<Farm>
{
    Task<IEnumerable<Farm>> GetAllFarms(string farmerId);
}