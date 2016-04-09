using System;
using System.Runtime.InteropServices;
using CSharpHDF5.Objects;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    internal static class AttributeHelper
    {
        public static ReadonlyList<Hdf5Attribute> GetAttributes(
            AbstractHdf5Object _object)
        {
            ulong n = 0;

            ReadonlyList<Hdf5Attribute> attributes = new ReadonlyList<Hdf5Attribute>();

            int id = H5A.iterate(_object.Id.Value, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref n,
                delegate(int _id, IntPtr _namePtr, ref H5A.info_t _ainfo, IntPtr _data)
                {
                    string attributeName = Marshal.PtrToStringAnsi(_namePtr);
                   
                    Hdf5Attribute attribute = GetStringAttribute(_object.Id, attributeName);
                    if (attribute != null)
                    {
                        attributes.Add(attribute);
                    }

                    return 0;
                }, new IntPtr());

            return attributes;
        }

        public static Hdf5Attribute GetStringAttribute(Hdf5Identifier _objectId, string _title)
        {
            int attributeId = 0;
            int typeId = 0;

            try
            {
                attributeId = H5A.open(_objectId.Value, _title);
                typeId = H5A.get_type(attributeId);
                var sizeData = H5T.get_size(typeId);
                var size = sizeData.ToInt32();
                byte[] strBuffer = new byte[size];

                var aTypeMem = H5T.get_native_type(typeId, H5T.direction_t.ASCEND);
                GCHandle pinnedArray = GCHandle.Alloc(strBuffer, GCHandleType.Pinned);
                H5A.read(attributeId, aTypeMem, pinnedArray.AddrOfPinnedObject());
                pinnedArray.Free();
                H5T.close(aTypeMem);

                string value = System.Text.Encoding.ASCII.GetString(strBuffer, 0, strBuffer.Length - 1);

                var attribute = new Hdf5Attribute
                {
                    Id = attributeId.ToId(),
                    Name = _title,
                    Value = value
                };

                return attribute;
            }
            catch (Exception ex)
            {
                //TODO - Log
                return null;
            }
            finally
            {
                if (attributeId > 0)
                {
                    H5A.close(attributeId);
                }

                if (typeId > 0)
                {
                    H5T.close(typeId);
                }
            }
        }

        public static bool CreateStringAttribute(int _objectId, string _title, string _description)
        {
            int attributeSpace = 0;
            int stringId = 0;
            int attributeId = 0;

            try
            {
                attributeSpace = H5S.create(H5S.class_t.SCALAR);
                stringId = H5T.copy(H5T.C_S1);
                H5T.set_size(stringId, new IntPtr(_description.Length));
                attributeId = H5A.create(_objectId, _title, stringId, attributeSpace);

                IntPtr descriptionArray = Marshal.StringToHGlobalAnsi(_description);
                H5A.write(attributeId, stringId, descriptionArray);

                Marshal.FreeHGlobal(descriptionArray);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (attributeId > 0)
                {
                    H5A.close(attributeId);
                }

                if (stringId > 0)
                {
                    H5T.close(stringId);
                }

                if (attributeSpace > 0)
                {
                    H5S.close(attributeSpace);
                }
            }
        }
    }
}
