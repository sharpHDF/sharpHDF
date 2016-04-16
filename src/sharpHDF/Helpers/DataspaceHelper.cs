using HDF.PInvoke;
using sharpHDF.Library.Objects;
using sharpHDF.Library.Structs;

namespace sharpHDF.Library.Helpers
{
    internal static class DataspaceHelper
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

            Hdf5Dataspace dataspace = new Hdf5Dataspace
            {
                Id = dataspaceId,
                NumberOfDimensions = rank
            };

            for (int i = 0; i < dims.Length; i++)
            {
                Hdf5DimensionProperty property = new Hdf5DimensionProperty
                {
                    CurrentSize = dims[i],
                    //MaximumSize = maxDims[i]
                };
                dataspace.DimensionProperties.Add(property);
            }

            H5S.close(dataspaceId.Value);

            return dataspace;
        }
    }
}
