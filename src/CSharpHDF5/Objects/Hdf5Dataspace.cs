using System.Collections.Generic;
using CSharpHDF5.Structs;

namespace CSharpHDF5.Objects
{
    public class Hdf5Dataspace
    {
        public Hdf5Dataspace()
        {
            DimensionProperties = new List<Hdf5DimensionProperty>();             
        }

        internal Hdf5Identifier Id { get; set; }

        public int NumberOfDimensions { get; set; }

        public List<Hdf5DimensionProperty> DimensionProperties { get; set; }
    }
}
