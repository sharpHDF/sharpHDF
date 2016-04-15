using System;
using System.Runtime.InteropServices;
using CSharpHDF5.Enums;
using CSharpHDF5.Exceptions;
using CSharpHDF5.Objects;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    internal static class AttributeHelper
    {
        public static void LoadAttributes(
            Hdf5Attributes _attributes)
        {
            ulong n = 0;

            AbstractHdf5Object obj =_attributes.ParentObject;

            int id = H5A.iterate(obj.Id.Value, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref n,
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
                                attribute = GetStringAttribute(obj.Id, attributeName);
                            }
                            else
                            {
                                attribute = GetAttribute(attributeId, attributeName, type);
                            }

                            if (attribute != null)
                            {
                                _attributes.Add(attribute);
                            }

                            H5T.close(attributeTypeId.Value);
                        }

                        H5A.close(attributeId.Value);
                    }

                    return 0;
                }, new IntPtr());
        }

        /// <summary>
        /// Assumes that parenent object is already open
        /// </summary>
        /// <param name="_objectId"></param>
        /// <param name="_title"></param>
        public static void DeleteAttribute(Hdf5Identifier _objectId, string _title)
        {
            int result = H5A.delete(_objectId.Value, _title);
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_objectId"></param>
        /// <param name="_attributes"></param>
        /// <param name="_title"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static Hdf5Attribute CreateAttributeAddToList<T>(
            Hdf5Identifier _objectId,
            ReadonlyList<Hdf5Attribute> _attributes,
            string _title,
            T _value)
        {
            Hdf5Attribute attribute = CreateAttribute(_objectId, _title, _value);

            if (attribute != null)
            {
                _attributes.Add(attribute);
            }

            return attribute;
        }

        /// <summary>
        /// Currently does not support arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_objectId"></param>
        /// <param name="_title"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static Hdf5Attribute CreateAttribute<T>(Hdf5Identifier _objectId, string _title, T _value)
        {
            ulong[] sizes = new ulong[1] {1};
            Hdf5Identifier dataspaceId;
            Hdf5Identifier attributeId;
            Hdf5Identifier typeId;
            Hdf5Attribute attribute = null;
            Hdf5DataType dataTypeObject = null;

            var datatype = TypeHelper.GetDataTypesEnum<T>();
            
            if (datatype != Hdf5DataTypes.String)
            {
                var tempType = TypeHelper.GetNativeType(datatype);

                typeId = H5T.copy(tempType.Value).ToId();
                dataTypeObject = TypeHelper.GetDataTypeByType(typeId);
                var status = H5T.set_order(typeId.Value, H5T.order_t.LE);

                dataspaceId = H5S.create_simple(1, sizes, null).ToId();

                attributeId = H5A.create(_objectId.Value, _title, typeId.Value, dataspaceId.Value).ToId();

                if (attributeId.Value > 0)
                {
                    WriteValue(dataTypeObject, attributeId, _value);
                }
            }
            else
            {
                string tempValue = Convert.ToString(_value);

                dataspaceId = H5S.create(H5S.class_t.SCALAR).ToId();
                typeId = H5T.copy(H5T.C_S1).ToId();
                int length = tempValue.Length + 1;
                var result = H5T.set_size(typeId.Value, new IntPtr(length));
                
                dataTypeObject = TypeHelper.GetDataTypeByType(typeId);

                attributeId = H5A.create(_objectId.Value, _title, typeId.Value, dataspaceId.Value).ToId();

                IntPtr valueArray = Marshal.StringToHGlobalAnsi(tempValue);
                result = H5A.write(attributeId.Value, typeId.Value, valueArray);
                Marshal.FreeHGlobal(valueArray);
            }

            attribute = new Hdf5Attribute
            {
                Value = _value,
                Name = _title,
                Id = attributeId
            };


            H5S.close(dataspaceId.Value);
            H5T.close(typeId.Value);
            H5A.close(attributeId.Value);

            return attribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_objectId"></param>
        /// <param name="_title"></param>
        /// <param name="_newValue"></param>
        /// <returns></returns>
        public static void UpdateAttribute<T>(Hdf5Identifier _objectId, string _title, T _newValue)
        {
            Hdf5Identifier attributeId = H5A.open(_objectId.Value, _title).ToId();

            if (attributeId.Value > 0)
            {
                Hdf5DataType type = TypeHelper.GetDataType(attributeId);
                Hdf5DataTypes enumType = TypeHelper.GetDataTypesEnum<T>();

                if (type.Type == enumType)
                {
                    WriteValue(type, attributeId, _newValue);    
                }
                else
                {
                    throw new Hdf5TypeMismatchException();
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
                return ReadValue<UInt16>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt32)
            {
                return ReadValue<UInt32>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.UInt64)
            {
                return ReadValue<UInt64>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Single)
            {
                return ReadValue<Single>(_dataType, _attributeId);
            }

            if (_dataType.Type == Hdf5DataTypes.Double)
            {
                return ReadValue<Double>(_dataType, _attributeId);
            }

            throw new Hdf5UnknownDataType();
        }

        private static void WriteValue<T>(
            Hdf5DataType _dataType,
            Hdf5Identifier _attributeId,
            T _value)
        {
            T[] value = new T[1];
            value[0] = _value;

            GCHandle arrayHandle = GCHandle.Alloc(value, GCHandleType.Pinned);

            var dataType = H5T.copy(_dataType.NativeType.Value).ToId();

            int result = H5A.write(_attributeId.Value, dataType.Value, arrayHandle.AddrOfPinnedObject());

            arrayHandle.Free();
            
            H5T.close(dataType.Value);
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
