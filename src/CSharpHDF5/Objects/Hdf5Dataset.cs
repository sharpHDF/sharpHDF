using System;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Objects
{
    /// <summary>
    /// Contains information around a HDF5 Dataset
    /// </summary>
    public class Hdf5Dataset : AbstractHdf5Object, IHasAttributes
    {
        internal Hdf5Dataset(Hdf5Identifier _fileId, Hdf5Identifier _id, string _path)
        {
            FileId = _fileId;
            Id = _id;

            Path = new Hdf5Path(_path);
            Name = Path.Name;

            m_Attributes = new Hdf5Attributes(this);
            AttributeHelper.LoadAttributes(m_Attributes);
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
        public void SetData<T>(T[] _array)
        {
            
        }

        public void SetData<T>(T[,] _array)
        {
            
        }

        private readonly Hdf5Attributes m_Attributes;

        /// <summary>
        /// List of attributes that are attached to this object
        /// </summary>
        public Hdf5Attributes Attributes
        {
            get
            {
                return m_Attributes;
            }
        }
    }
}
