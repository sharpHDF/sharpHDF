using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IHasAttributes
    {
        Hdf5Attributes Attributes { get; }
    }
}
