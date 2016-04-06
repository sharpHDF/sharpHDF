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
            Groups = new List<Hdf5Group>();
            Datasets = new List<Hdf5Dataset>();
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

            Groups = new List<Hdf5Group>();                       
            Datasets = new List<Hdf5Dataset>();
        }

        public string Name { get; set; }

        public List<Hdf5Group> Groups { get; set; }

        public List<Hdf5Dataset> Datasets { get; set; } 

        public List<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(this); }
        }

        internal void LoadChildObjects()
        {
            GroupHelper.PopulateChildrenObjects(m_FileId, this);
        }

        public Hdf5Group NewGroup(string _name)
        {
            return null;
        }

        public Hdf5Dataset NewDataset(string _name, Hdf5DataTypes _datatype, int _dimensions)
        {
            return null;
        }
    }
}
