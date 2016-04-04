using System;
using System.Collections.Generic;
using System.IO;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;
using HDF.PInvoke;

namespace CSharpHDF5.Objects
{
    public class Hdf5File : IDisposable, IGroupManagement, IAttributeManagement
    {
        private int m_FileId;

        public Hdf5File(string _filename)
        {
            if (File.Exists(_filename))
            {
                m_FileId = H5F.open(_filename, H5F.ACC_RDWR);
            }
            else
            {
                m_FileId = H5F.create(_filename, H5F.ACC_TRUNC);
            }
        }

        /// <summary>
        /// Close the file
        /// </summary>
        public void Close()
        {
            H5F.close(m_FileId);
            m_FileId = 0;
        }

        public List<Hdf5Group> Groups
        {
            get { return GroupHelper.GetGroups(m_FileId); }
        }

        public List<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(m_FileId); }
        }

        public void Dispose()
        {
            if (m_FileId != 0)
            {
                Close();
            }            
        }
    }
}
