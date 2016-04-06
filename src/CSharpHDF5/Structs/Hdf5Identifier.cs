using System;

namespace CSharpHDF5.Structs
{
    public struct Hdf5Identifier
    {
#if HDF5_VER1_10
        public Int64 Value;
#else
        public Int32 Value;
#endif
    }
}
