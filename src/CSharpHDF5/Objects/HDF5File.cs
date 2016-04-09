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
    public class Hdf5File : AbstractHdf5Object, IDisposable, IHasGroups, IHasDatasets, IHasAttributes
    {
        /// <summary>
        /// Opens an existing file and loads up the group and dataset headers in the object.
        /// </summary>
        /// <param name="_filename"></param>
        public Hdf5File(string _filename)
        {
            if (!File.Exists(_filename))
            {
                throw new FileNotFoundException("Check file path, or use static Hdf5File.CreateFile to create new one.");
            }

            Id = H5F.open(_filename, H5F.ACC_RDWR).ToId();

            if (Id.Value > 0)
            {
                Path = new Hdf5Path(".");

                Groups = new ReadonlyList<Hdf5Group>();
                Datasets = new ReadonlyList<Hdf5Dataset>();

                GroupHelper.PopulateChildrenObjects(Id, this);
            }
            else
            {
                throw new Exception("Unknown exception opening file");
            }
        }

        public static Hdf5File CreateFile(string _filename)
        {
            if (File.Exists(_filename))
            {
                throw new Exception("File already exists.");
            }

            Hdf5Identifier fileId = H5F.create(_filename, H5F.ACC_CREAT).ToId();
            H5F.close(fileId.Value);

            return new Hdf5File(_filename);
        }

        /// <summary>
        /// Close the file
        /// </summary>
        public void Close()
        {
            H5F.close(Id.Value);
            Id = 0.ToId();
        }

        private ReadonlyList<Hdf5Attribute> m_Attributes = null; 

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

        /// <summary>
        /// List of the groups that are contained at the top level of the file
        /// </summary>
        public ReadonlyList<Hdf5Group> Groups { get; internal set; }

        /// <summary>
        /// List of the datasets that are contained at the top level of the file
        /// </summary>
        public ReadonlyList<Hdf5Dataset> Datasets { get; internal set; } 

        /// <summary>
        /// Disposes of object references in the file
        /// </summary>
        public void Dispose()
        {
            if (Id.Value != 0)
            {
                Close();
            }            
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
