
using CSharpHDF5.Helpers;
using CSharpHDF5.Structs;

namespace CSharpHDF5.Objects
{
    public class Hdf5Groups : ReadonlyList<Hdf5Group>
    {
        internal AbstractHdf5Object ParentObject { get; private set; }

        internal Hdf5Groups(
            AbstractHdf5Object _parentObject)
        {
            ParentObject = _parentObject;
        }


        /// <summary>
        /// Adds a group
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        public Hdf5Group Add(string _name)
        {
            return GroupHelper.CreateGroupAddToList(
                this,
                ParentObject.FileId,
                ParentObject.Path,
                _name);
        }

    }
}
