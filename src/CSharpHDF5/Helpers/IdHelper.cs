using System;
using CSharpHDF5.Struct;

namespace CSharpHDF5.Helpers
{
    public static class IdHelper
    {
#if HDF5_VER1_10
        public static Hdf5Identifier ToId(this Int64 _value)
#else
        public static Hdf5Identifier ToId(this Int32 _value)
#endif
        {
            var id = new Hdf5Identifier {Value = _value};
            return id;
        }
    }
}
