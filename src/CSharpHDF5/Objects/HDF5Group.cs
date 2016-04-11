using System.Collections.Generic;
using System.Text;
using CSharpHDF5.Enums;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Objects
{
    public class Hdf5Group : AbstractHdf5Object, IHasGroups, IHasAttributes, IHasDatasets
    {
        private Hdf5Identifier m_FileId;

        public Hdf5Group()
        {
            Groups = new ReadonlyList<Hdf5Group>();
            Datasets = new ReadonlyList<Hdf5Dataset>();
        }

        internal Hdf5Group(
            Hdf5Identifier _fileId, 
            Hdf5Identifier _groupId, 
            string _path)
        {
            m_FileId = _fileId;
            Id = _groupId;

            Path = new Hdf5Path(_path);
            Name = Path.Name;

            Groups = new ReadonlyList<Hdf5Group>();                      
            Datasets = new ReadonlyList<Hdf5Dataset>();
            m_Attributes = new ReadonlyList<Hdf5Attribute>();
        }

        public string Name { get; set; }

        public ReadonlyList<Hdf5Group> Groups { get; set; }

        public ReadonlyList<Hdf5Dataset> Datasets { get; set; }

        private ReadonlyList<Hdf5Attribute> m_Attributes;

        /// <summary>
        /// List of attributes that are attached to this object
        /// </summary>
        public ReadonlyList<Hdf5Attribute> Attributes
        {
            get
            {
                if (m_Attributes == null)
                {
                    m_Attributes = AttributeHelper.GetAttributes(this);
                }

                return m_Attributes;
            }
        }

        public Hdf5Attribute AddAttribute<T>(string _name, T _value)
        {
            Hdf5Attribute attribute = null; 
            
            var id = H5G.open(m_FileId.Value, Path.FullPath);
            if (id > 0)
            {
                attribute = AttributeHelper.CreateAttributeAddToList(Id, m_Attributes, _name, _value);
                H5G.close(id);
            }

            return attribute;
        }

        public void DeleteAttribute(Hdf5Attribute _attribute)
        {
            var id = H5G.open(Id.Value, Path.FullPath);

            if (id > 0)
            {
                AttributeHelper.DeleteAttribute(Id, _attribute.Name);

                m_Attributes.Remove(_attribute);

                H5G.close(id);
            }            
        }

        internal void LoadChildObjects()
        {
            GroupHelper.PopulateChildrenObjects(m_FileId, this);
        }

        /// <summary>
        /// Adds a group to the root level of the file.
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        public Hdf5Group AddGroup(string _name)
        {
            return GroupHelper.CreateGroupAddToList(Groups, m_FileId, Path, _name);
        }

        /// <summary>
        /// Adds a dataset to the root level of the file.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_datatype"></param>
        /// <param name="_numberOfDimensions"></param>
        /// <param name="_dimensionProperties"></param>
        /// <returns></returns>
        public Hdf5Dataset AddDataset(
            string _name,
            Hdf5DataTypes _datatype,
            int _numberOfDimensions,
            List<Hdf5DimensionProperty> _dimensionProperties)
        {
            return DatasetHelper.CreateDatasetAddToDatasets(
                Datasets,
                m_FileId,
                Path,
                _name,
                _datatype,
                _numberOfDimensions,
                _dimensionProperties);
        }
    }
}
