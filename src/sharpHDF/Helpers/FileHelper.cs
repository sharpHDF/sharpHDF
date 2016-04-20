/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using HDF.PInvoke;
using sharpHDF.Library.Structs;

namespace sharpHDF.Library.Helpers
{
    public static class FileHelper
    {
        public static void FlushToFile(Hdf5Identifier _fileId)
        {
            H5F.flush(_fileId.Value, H5F.scope_t.GLOBAL);
        }
    }
}
