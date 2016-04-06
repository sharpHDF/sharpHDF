using System;
using System.Collections.Generic;
using System.IO;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
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

        public Hdf5Group NewGroup(string _name)
        {
            return null;
        }
    }
}
