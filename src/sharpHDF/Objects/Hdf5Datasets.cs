using System.Collections.Generic;
using sharpHDF.Library.Enums;
using sharpHDF.Library.Helpers;

namespace sharpHDF.Library.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class Hdf5Datasets : ReadonlyList<Hdf5Dataset>
    {
        internal AbstractHdf5Object ParentObject { get; private set; }

        internal Hdf5Datasets(
            AbstractHdf5Object _parentObject)
        {
            ParentObject = _parentObject;
        }

        /// <summary>
        /// Adds a dataset
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_datatype"></param>
        /// <param name="_dimensionProperties"></param>
        /// <returns></returns>
        public Hdf5Dataset Add(
            string _name,
            Hdf5DataTypes _datatype,            
            List<Hdf5DimensionProperty> _dimensionProperties)
        {
            int numberOfDimensions = _dimensionProperties.Count;

            return DatasetHelper.CreateDatasetAddToDatasets(
                this,
                ParentObject.FileId,
                ParentObject.Path,
                _name,
                _datatype,
                numberOfDimensions,
                _dimensionProperties);
        }
    }
}
