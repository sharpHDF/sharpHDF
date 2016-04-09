using System;
using CSharpHDF5.Helpers;
using CSharpHDF5.Structs;

namespace CSharpHDF5.Objects
{
    /// <summary>
    /// Contains information around a HDF5 Dataset
    /// </summary>
    public class Hdf5Dataset : AbstractHdf5Object
    {
        public Hdf5Dataset()
        {
        }

        internal Hdf5Dataset(Hdf5Identifier _id, string _path)
        {
            Id = _id;

            Path = new Hdf5Path(_path);
            Name = Path.Name;
        }

        /// <summary>
        /// Name of the Dataset.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data type that this dataset contains
        /// </summary>
        public Hdf5DataType DataType { get; set; }

        /// <summary>
        /// Information regarding the shape of the dataset
        /// </summary>
        public Hdf5Dataspace Dataspace { get; set; }
        
        /// <summary>
        /// Retreives an array of the data that the dataset contains
        /// </summary>
        /// <returns></returns>
        public Array GetData()
        {
            return DatasetHelper.GetData(this);
        }

        /// <summary>
        /// Saves an array of data to the dataset
        /// </summary>
        /// <param name="_array"></param>
        public void SetData(Array _array)
        {
            
        }
    }
}
