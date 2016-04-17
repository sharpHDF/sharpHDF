/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using HDF.PInvoke;
using sharpHDF.Library.Exceptions;
using sharpHDF.Library.Helpers;

namespace sharpHDF.Library.Objects
{
    /// <summary>
    /// Collection of attributes
    /// </summary>
    public class Hdf5Attributes :  ReadonlyList<Hdf5Attribute>
    {
        internal AbstractHdf5Object ParentObject { get; private set; }

        internal Hdf5Attributes(
            AbstractHdf5Object _parentObject)
        {
            ParentObject = _parentObject;
        }

        /// <summary>
        /// Checks if the attribute exists in the list with specified name.
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        public bool Exists(string _name)
        {
            foreach (Hdf5Attribute hdf5Attribute in this)
            {
                if (hdf5Attribute.Name == _name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add an attribute and write the values to the file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_name"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public Hdf5Attribute Add<T>(string _name, T _value)
        {
            if (Exists(_name))
            {
                throw new Hdf5AttributeAlreadyExistException();
            }

            //If the parent is the file, then just create the attribute (file already open).
            if (ParentObject.Id.Equals(ParentObject.FileId))
            {
                return AttributeHelper.CreateAttributeAddToList(ParentObject.Id, this, _name, _value);
            }
            
            Hdf5Attribute attribute = null;

            //Otherwise open the object and then call the function.
            var id = H5O.open(ParentObject.FileId.Value, ParentObject.Path.FullPath).ToId();
            if (id.Value > 0)
            {
                attribute = AttributeHelper.CreateAttributeAddToList(id, this, _name, _value);
                H5O.close(id.Value);
            }

            return attribute;
        }

        /// <summary>
        /// Update the attribute and write the values to the file
        /// </summary>
        /// <param name="_attribute"></param>
        public void Update(Hdf5Attribute _attribute)
        {
            //If the parent is the file, then just update the attribute (file already open).
            if (ParentObject.Id.Equals(ParentObject.FileId))
            {
                AttributeHelper.UpdateAttribute(ParentObject.Id, _attribute);
            }

            //Otherwise open the object and then update the attribute.
            var id = H5O.open(ParentObject.FileId.Value, ParentObject.Path.FullPath).ToId();
            if (id.Value > 0)
            {
                AttributeHelper.UpdateAttribute(id, _attribute);
                H5O.close(id.Value);
            }
        }

        /// <summary>
        /// Remove the attribute from the list and delete the attibute from the file
        /// </summary>
        /// <param name="_attribute"></param>
        public void Delete(Hdf5Attribute _attribute)
        {
            //If the parent is the file, then just delete the attribute (file already open).
            if (ParentObject.Id.Equals(ParentObject.FileId))
            {
                AttributeHelper.DeleteAttribute(ParentObject.Id, _attribute.Name);
            }
            else
            {
                //Otherwise open the object then delete the attibute
                var id = H5O.open(ParentObject.FileId.Value, ParentObject.Path.FullPath).ToId();
                if (id.Value > 0)
                {
                    AttributeHelper.DeleteAttribute(id, _attribute.Name);
                    H5O.close(id.Value);
                }
            }

            Remove(_attribute);
        }
    }
}
