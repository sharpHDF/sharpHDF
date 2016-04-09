using System.Collections.Generic;
using CSharpHDF5.Enums;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Interfaces
{
    public interface IHasDatasets
    {
        ReadonlyList<Hdf5Dataset> Datasets { get; }

        Hdf5Dataset AddDataset(
            string _name,
            Hdf5DataTypes _datatype,
            int _numberOfDimensions,
            List<Hdf5DimensionProperty> _dimensionProperties);
    }
}
