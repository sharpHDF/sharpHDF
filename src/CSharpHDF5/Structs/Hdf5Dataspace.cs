namespace CSharpHDF5.Structs
{
    public struct Hdf5Dataspace
    {
        internal Hdf5Identifier Id { get; set; }

        public int NumberOfDimensions { get; set; }

        public int CurrentSize { get; set; }

        public int MaxSize { get; set; }
    }
}
