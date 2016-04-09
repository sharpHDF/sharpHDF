using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasGroups
    {
        ReadonlyList<Hdf5Group> Groups { get; }

        Hdf5Group AddGroup(string _name);
    }
}
