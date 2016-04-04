using System.Collections.Generic;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IGroupManagement
    {
        List<Hdf5Group> Groups { get; }
    }
}
