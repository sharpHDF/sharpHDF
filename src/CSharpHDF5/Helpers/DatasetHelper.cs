using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpHDF5.Objects;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    public static class DatasetHelper
    {
        /// <summary>
        /// Assumes that dataset already open
        /// </summary>
        /// <param name="_fileId"></param>
        /// <param name="_datasetId"></param>
        /// <returns></returns>
        public static Hdf5Dataset LoadDataset(Hdf5Identifier _fileId, Hdf5Identifier _datasetId, string _fullPath)
        {
            Hdf5DataType datatype = TypeHelper.GetDataType(_datasetId);
            Hdf5Dataspace dataspace = DataspaceHelper.GetDataspace(_datasetId);

            Hdf5Dataset dataset = new Hdf5Dataset(_fileId, _fullPath)
            {
                FileId = _fileId,
                Dataspace = dataspace,
                Type = datatype
            };

            return dataset;
        }

        public static Array GetData(Hdf5Dataset _dataset)
        {
            var id = H5O.open(_dataset.FileId.Value, _dataset.Path.FullPath);

            if (id > 0)
            {
                Array a;

                H5O.close(id);
            }

            return null;
        }
    }
}
