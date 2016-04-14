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
        internal Hdf5Dataset(Hdf5Identifier _id, string _path)
        {
            Id = _id;

            Path = new Hdf5Path(_path);
            Name = Path.Name;
            m_Attributes = AttributeHelper.GetAttributes(this);
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

        private readonly ReadonlyList<Hdf5Attribute> m_Attributes;

        /// <summary>
        /// List of attributes that are attached to this object
        /// </summary>
        public ReadonlyList<Hdf5Attribute> Attributes
        {
            get
            {
                return m_Attributes;
            }
        }

        public Hdf5Attribute AddAttribute<T>(string _name, T _value)
        {
            Hdf5Attribute attribute = null;

            var id = H5O.open(FileId.Value, Path.FullPath);
            if (id > 0)
            {
                attribute = AttributeHelper.CreateAttributeAddToList(id.ToId(), m_Attributes, _name, _value);
                H5O.close(id);
            }

            return attribute;
        }

        public void DeleteAttribute(Hdf5Attribute _attribute)
        {
            var id = H5O.open(FileId.Value, Path.FullPath).ToId();
            if (id.Value > 0)
            {
                AttributeHelper.DeleteAttribute(id, _attribute.Name);

                m_Attributes.Remove(_attribute);
                H5O.close(id.Value);
            }
        }
    }
}
