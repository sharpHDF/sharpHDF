using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasGroups
    {
        Hdf5Groups Groups { get; }
    }
}
