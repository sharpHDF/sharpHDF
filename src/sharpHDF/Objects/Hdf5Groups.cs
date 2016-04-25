/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using sharpHDF.Library.Helpers;

namespace sharpHDF.Library.Objects
{
    public class Hdf5Groups : ReadonlyNamedItemList<Hdf5Group>
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
