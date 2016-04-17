/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using sharpHDF.Library.Structs;

namespace sharpHDF.Library.Helpers
{
    internal static class IdHelper
    {
#if HDF5_VER1_10
        public static Hdf5Identifier ToId(this Int64 _value)
#else
        public static Hdf5Identifier ToId(this Int32 _value)
#endif
        {
            var id = new Hdf5Identifier(_value);
            return id;
        }
    }
}
