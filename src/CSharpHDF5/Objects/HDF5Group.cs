using System.Collections.Generic;
using CSharpHDF5.Helpers;
using CSharpHDF5.Interfaces;

namespace CSharpHDF5.Objects
{
    public class Hdf5Group : IGroupManagement, IAttributeManagement
    {
        private int m_GroupId;

        public List<Hdf5Group> Groups
        {
            get
            {
                return GroupHelper.GetGroups(m_GroupId);   
            }            
        }

        public List<Hdf5Attribute> Attributes
        {
            get { return AttributeHelper.GetAttributes(m_GroupId); }
        }
    }
}
