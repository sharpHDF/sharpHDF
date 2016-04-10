using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IHasAttributes
    {
        ReadonlyList<Hdf5Attribute> Attributes { get; }


    }
}
