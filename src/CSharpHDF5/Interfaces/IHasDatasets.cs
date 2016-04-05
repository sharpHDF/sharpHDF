using System.Collections.Generic;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasDatasets
    {
        List<Hdf5Dataset> Datasets { get; set; }
    }
}
