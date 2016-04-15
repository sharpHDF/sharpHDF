using System.Collections.Generic;
using CSharpHDF5.Enums;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Structs;

namespace CSharpHDF5.Objects
{
    public class Hdf5Group : AbstractHdf5Object, IHasGroups, IHasAttributes, IHasDatasets
    {
        internal Hdf5Group(
            Hdf5Identifier _fileId, 
            Hdf5Identifier _groupId, 
            string _path)
        {
            FileId = _fileId;
            Id = _groupId;

            Path = new Hdf5Path(_path);
            Name = Path.Name;

            Groups = new Hdf5Groups(this);
            Datasets = new Hdf5Datasets(this);

            Attributes = new Hdf5Attributes(this);
            AttributeHelper.LoadAttributes(Attributes);
        }

        public string Name { get; set; }

        public Hdf5Groups Groups { get; set; }

        public Hdf5Datasets Datasets { get; set; }


        /// <summary>
        /// List of attributes that are attached to this object
        /// </summary>
        public Hdf5Attributes Attributes { get; private set; }

        internal void LoadChildObjects()
        {
            GroupHelper.PopulateChildrenObjects(FileId, this);
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
                FileId,
                Path,
                _name,
                _datatype,
                _numberOfDimensions,
                _dimensionProperties);
        }
    }
}
