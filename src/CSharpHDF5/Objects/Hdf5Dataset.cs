using System;
using CSharpHDF5.Helpers;
using CSharpHDF5.Structs;

namespace CSharpHDF5.Objects
{
    public class Hdf5Dataset : AbstractHdf5Object
    {
        public string Name { get; set; }

        public Hdf5DataType Type { get; set; }

        public Hdf5Dataspace Dataspace { get; set; }

        public Hdf5Dataset()
        {
        }

        internal Hdf5Dataset(Hdf5Identifier _id, string _path)
        {
            Id = _id;

            Path = new Hdf5Path(_path);
            Name = Path.Name;
        }


        public Array GetData()
        {
            return DatasetHelper.GetData(this);
        }
    }
}
