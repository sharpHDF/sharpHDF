using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasDatasets
    {
        Hdf5Datasets Datasets { get; }
    }
}
