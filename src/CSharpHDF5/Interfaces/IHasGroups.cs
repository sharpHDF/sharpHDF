using System.Collections.Generic;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasGroups
    {
        List<Hdf5Group> Groups { get; }
    }
}
