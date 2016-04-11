using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IHasAttributes
    {
        ReadonlyList<Hdf5Attribute> Attributes { get; }

        Hdf5Attribute AddAttribute<T>(string _name, T _value);

        void DeleteAttribute(Hdf5Attribute _attribute);
    }
}
