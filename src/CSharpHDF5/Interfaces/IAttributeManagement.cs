using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    interface IAttributeManagement
    {
        List<Hdf5Attribute> Attributes { get; }
    }
}
