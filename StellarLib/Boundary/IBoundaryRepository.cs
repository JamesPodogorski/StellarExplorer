using System;

namespace StellarLib;

public interface IBoundaryRepository : IRepository<Boundary>
{
    Task<IEnumerable<Boundary>> GetAll();
}
