using System.Collections.Generic;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IHasAttributes
    {
        List<Hdf5Attribute> Attributes { get; }
    }
}
