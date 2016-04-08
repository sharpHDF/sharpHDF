using System;
using System.Collections.Generic;
using System.IO;
using CSharpHDF5.Enums;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Objects
{
    public class Hdf5File : AbstractHdf5Object, IDisposable, IHasGroups, IHasAttributes
    {
        public Hdf5File(string _filename)
        {
            if (File.Exists(_filename))
            {
                Id = H5F.open(_filename, H5F.ACC_RDWR).ToId();
            }
            else
            {
                Id = H5F.create(_filename, H5F.ACC_TRUNC).ToId();
            }

            Path = new Hdf5Path(".");

            Groups = new List<Hdf5Group>();

            GroupHelper.PopulateChildrenObjects(Id, this);
        }

        /// <summary>
        /// Close the file
        /// </summary>
        public void Close()
        {
            H5F.close(Id.Value);
            Id = 0.ToId();
        }

        public List<Hdf5Group> Groups { get; set; }
        public List<Hdf5Dataset> Datasets { get; set; } 

        public List<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(this); }
        }

        public void Dispose()
        {
            if (Id.Value != 0)
            {
                Close();
            }            
        }

        public Hdf5Group AddGroup(string _name)
        {
            Hdf5Group group = GroupHelper.CreateGroup(Id, Path, _name);

            if (group != null)
            {
                Groups.Add(group);
            }
            return group;
        }

        public Hdf5Dataset AddDataset(
            string _name,
            Hdf5DataTypes _datatype,
            int _numberOfDimensions,
            List<Hdf5DimensionProperty> _dimensionProperties)
        {
            Hdf5Dataset dataset = DatasetHelper.CreateDataset(
                Id, Path, _name, _datatype,
                _numberOfDimensions, _dimensionProperties);

            if (dataset != null)
            {
                Datasets.Add(dataset);
            }

            return dataset;
        }
    }
}
