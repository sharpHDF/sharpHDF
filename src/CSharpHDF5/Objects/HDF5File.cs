using System;
using System.IO;
using CSharpHDF5.Exceptions;
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
                throw new Hdf5FileNotFoundException();
            }

            Id = H5F.open(_filename, H5F.ACC_RDWR).ToId();

            if (Id.Value > 0)
            {
                FileId = Id;
                Path = new Hdf5Path(".");

                Groups = new Hdf5Groups(this);
                Datasets = new Hdf5Datasets(this);

                Attributes = new Hdf5Attributes(this);
                AttributeHelper.LoadAttributes(Attributes);

                GroupHelper.PopulateChildrenObjects(Id, this);
            }
            else
            {
                throw new Hdf5UnknownException();
            }
        }

        public static Hdf5File Create(string _filename)
        {
            if (File.Exists(_filename))
            {
                throw new Hdf5FileExistsException();
            }

            Hdf5Identifier fileId = H5F.create(_filename, H5F.ACC_EXCL).ToId();

            if (fileId.Value > 0)
            {
                H5F.close(fileId.Value);
                return new Hdf5File(_filename);
            }

            throw new Hdf5UnknownException();
        }

        /// <summary>
        /// Close the file
        /// </summary>
        public void Close()
        {
            H5F.close(Id.Value);
            Id = 0.ToId();
        }

        /// <summary>
        /// List of attributes that are attached to this object
        /// </summary>
        public Hdf5Attributes Attributes {get; private set; }

        /// <summary>
        /// List of the groups that are contained at the top level of the file
        /// </summary>
        public Hdf5Groups Groups { get; internal set; }

        /// <summary>
        /// List of the datasets that are contained at the top level of the file
        /// </summary>
        public Hdf5Datasets Datasets { get; internal set; } 

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
    }
}
