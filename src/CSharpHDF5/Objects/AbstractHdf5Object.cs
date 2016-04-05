namespace CSharpHDF5.Objects
{
    public abstract class AbstractHdf5Object
    {
        internal int Id { get; set; }

        internal Hdf5Path Path { get; set; }
    }
}
