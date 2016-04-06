﻿using CSharpHDF5.Struct;

namespace CSharpHDF5.Objects
{
    public abstract class AbstractHdf5Object
    {
        internal Hdf5Identifier Id { get; set; }

        internal Hdf5Path Path { get; set; }
    }
}