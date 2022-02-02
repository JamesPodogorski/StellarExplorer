using System;

namespace StellarLib;

public interface IFarmMember
{
    string identity { get; set; }
    IFarmMember Parent { get; set; }

    IFarmMember GetRoot(IFarmMember farmMember)
    {
        if (farmMember.Parent == null)
        {
            return farmMember;
        }
        else
        {
            return GetRoot(farmMember.Parent);
        }
    }
}
