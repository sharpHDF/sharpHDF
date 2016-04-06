using CSharpHDF5.Objects;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    public static class DataspaceHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Hdf5Dataspace GetDataspace(Hdf5Identifier _datasetId)
        {
            var dataspaceId = H5D.get_space(_datasetId.Value).ToId();

            int rank = H5S.get_simple_extent_ndims(dataspaceId.Value);
            ulong[] dims = new ulong[rank];
            ulong[] maxDims = new ulong[rank];
            H5S.get_simple_extent_dims(dataspaceId.Value, dims, maxDims);

            Hdf5Dataspace dataspace = new Hdf5Dataspace();
            dataspace.Id = dataspaceId;
            dataspace.NumberOfDimensions = rank;

            H5S.close(dataspaceId.Value);

            return dataspace;
        }
    }
}
