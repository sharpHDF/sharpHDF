/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace sharpHDF.Library.Exceptions
{
    /// <summary>
    /// This library does not support this datatype, but HDF5 may.
    /// </summary>
    public class Hdf5UnsupportedDataTypeException : Exception
    {
    }
}
