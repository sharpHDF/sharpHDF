using HDF.PInvoke;
using sharpHDF.Library.Helpers;

namespace sharpHDF.Library.Objects
{
    public class Hdf5Attributes :  ReadonlyList<Hdf5Attribute>
    {
        internal AbstractHdf5Object ParentObject { get; private set; }

        internal Hdf5Attributes(
            AbstractHdf5Object _parentObject)
        {
            ParentObject = _parentObject;
        }

        public Hdf5Attribute Add<T>(string _name, T _value)
        {
            if (ParentObject.Id.Equals(ParentObject.FileId))
            {
                return AttributeHelper.CreateAttributeAddToList(ParentObject.Id, this, _name, _value);
            }
            
            Hdf5Attribute attribute = null;

            var id = H5O.open(ParentObject.FileId.Value, ParentObject.Path.FullPath);
            if (id > 0)
            {
                attribute = AttributeHelper.CreateAttributeAddToList(id.ToId(), this, _name, _value);
                H5O.close(id);
            }

            return attribute;
        }

        public void Delete(Hdf5Attribute _attribute)
        {
            if (ParentObject.Id.Equals(ParentObject.FileId))
            {
                AttributeHelper.DeleteAttribute(ParentObject.Id, _attribute.Name);

                Remove(_attribute);
                return;
            }

            var id = H5O.open(ParentObject.FileId.Value, ParentObject.Path.FullPath).ToId();
            if (id.Value > 0)
            {
                AttributeHelper.DeleteAttribute(id, _attribute.Name);

                Remove(_attribute);
                H5O.close(id.Value);
            }
        }
    }
}
