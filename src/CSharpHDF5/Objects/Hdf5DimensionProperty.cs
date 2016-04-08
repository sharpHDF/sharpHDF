namespace CSharpHDF5.Objects
{
    public class Hdf5DimensionProperty
    {
        public Hdf5DimensionProperty()
        {
            CurrentSize = 1;
            MaximumSize = ulong.MaxValue;
        }

        public ulong CurrentSize { get; set; }
        public ulong MaximumSize { get; set; }
    }
}
