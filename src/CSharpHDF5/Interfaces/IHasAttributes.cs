using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IHasAttributes
    {
        ReadonlyList<Hdf5Attribute> Attributes { get; }

        void AddAttribute<T>(string _name, T _value);

        void DeleteAttribute(string _name);
    }
}
