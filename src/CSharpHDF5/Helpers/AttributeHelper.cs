using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSharpHDF5.Objects;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    public static class AttributeHelper
    {
        public static List<Hdf5Attribute> GetAttributes(
            int _objectId)
        {
            ulong n = 0;

            List<Hdf5Attribute> attributes = new List<Hdf5Attribute>();

            int id = H5A.iterate(_objectId, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref n,
                delegate(int _id, IntPtr _namePtr, ref H5A.info_t _ainfo, IntPtr _data)
                {
                    string objectName = Marshal.PtrToStringAnsi(_namePtr);



                    return 0;
                }, new IntPtr());

            return attributes;
        }

    }
}
