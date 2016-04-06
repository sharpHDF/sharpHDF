using CSharpHDF5.Struct;

namespace CSharpHDF5.Objects
{
    public class Hdf5Dataset : AbstractHdf5Object
    {

        public Hdf5Dataset()
        {
        }

        internal Hdf5Dataset(Hdf5Identifier _id, string _path)
        {
            Id = _id;

            Path = new Hdf5Path(_path);
        }
    }
}
