using CSharpHDF5.Enums;

namespace CSharpHDF5.Structs
{
    public struct Hdf5DataType 
    {
        internal Hdf5Identifier Id { get; set; }

        internal Hdf5Identifier NativeType { get; set; }

        public Hdf5DataTypes Type { get; set; }

        public int Size { get; set; }

        public Hdf5ByteOrder ByteOrder { get; set; }

        public bool Unsigned { get; set; }
    }
}
