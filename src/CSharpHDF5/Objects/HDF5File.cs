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
                Id = H5F.open(_filename, H5F.ACC_RDWR);
            }
            else
            {
                Id = H5F.create(_filename, H5F.ACC_TRUNC);
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
            H5F.close(Id);
            Id = 0;
        }

        public List<Hdf5Group> Groups { get; set; }

        public List<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(this); }
        }

        public void Dispose()
        {
            if (Id != 0)
            {
                Close();
            }            
        }
    }
}
