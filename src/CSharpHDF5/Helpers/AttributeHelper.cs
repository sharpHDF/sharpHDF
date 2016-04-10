using System;
using System.Runtime.InteropServices;
using CSharpHDF5.Enums;
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

                    var attributeId = H5A.open(_id, attributeName).ToId();
                    if (attributeId.Value > 0)
                    {
                        var attributeTypeId = H5A.get_type(attributeId.Value).ToId();
                        var type = TypeHelper.GetDataTypeByType(attributeTypeId);
                     
                        if (attributeTypeId.Value > 0)
                        {
                            Hdf5Attribute attribute = null;
                            if (type.NativeType.Value == H5T.C_S1)
                            {
                                attribute = GetStringAttribute(_object.Id, attributeName);
                            }
                            else
                            {
                                attribute = GetAttribute(attributeId, attributeName, type);
                            }                                

                            if (attribute != null)
                            {
                                attributes.Add(attribute);
                            }

                            H5T.close(attributeTypeId.Value);
                        }

                        H5A.close(attributeId.Value);
                    }

                    return 0;
                }, new IntPtr());

            return attributes;
        }

        public static Hdf5Attribute GetAttribute(Hdf5Identifier _attributeId, string _title, Hdf5DataType _type)
        {
            Object value = ReadValue(_type, _attributeId);

            var attribute = new Hdf5Attribute
            {
                Id = _attributeId,
                Name = _title,
                Value = value
            };

            return attribute;
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



        public static Object ReadValue(
            Hdf5DataType _dataType,
            Hdf5Identifier _attributeId)
        {
            if (_dataType.Type == Hdf5DataTypes.Int8)
            {
                return ReadValue<sbyte>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Int16)
            {
                return ReadValue<Int16>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Int32)
            {
                return ReadValue<Int32>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Int64)
            {
                return ReadValue<Int64>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt8)
            {
                return ReadValue<byte>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt16)
            {
                return ReadValue<byte>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt32)
            {
                return ReadValue<byte>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt64)
            {
                return ReadValue<byte>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Single)
            {
                return ReadValue<Single>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Double)
            {
                return ReadValue<Double>(_dataType, _attributeId);
            }

            return null;
        }

        private static T ReadValue<T>(
            Hdf5DataType _dataType,
            Hdf5Identifier _attributeId)
        {
            T[] value = new T[1];

            GCHandle arrayHandle = GCHandle.Alloc(value, GCHandleType.Pinned);

            var dataType = H5T.copy(_dataType.NativeType.Value).ToId();

            int result = H5A.read(_attributeId.Value, dataType.Value, arrayHandle.AddrOfPinnedObject());

            arrayHandle.Free();

            H5T.close(dataType.Value);

            return value[0];
        }
    }
}
