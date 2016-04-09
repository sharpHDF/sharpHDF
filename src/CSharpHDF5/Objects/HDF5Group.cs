using System.Collections.Generic;
using System.Text;
using CSharpHDF5.Enums;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Structs;

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
        }

        public string Name { get; set; }

        public ReadonlyList<Hdf5Group> Groups { get; set; }

        public ReadonlyList<Hdf5Dataset> Datasets { get; set; } 

        public ReadonlyList<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(this); }
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
            return GroupHelper.CreateGroupAddToList(Groups, Id, Path, _name);
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
                Id,
                Path,
                _name,
                _datatype,
                _numberOfDimensions,
                _dimensionProperties);
        }
    }
}
