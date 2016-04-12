using System;
using CSharpHDF5.Enums;
using CSharpHDF5.Exceptions;
using CSharpHDF5.Objects;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    internal static class TypeHelper
    {

        public static Hdf5DataType GetDataTypeByType(Hdf5Identifier _typeId)
        {
            var typeClass = H5T.get_class(_typeId.Value);
            var typeSize = (int)H5T.get_size(_typeId.Value);
            var typeSign = H5T.get_sign(_typeId.Value);
            var typeOrder = H5T.get_order(_typeId.Value);

            Hdf5DataType dt = new Hdf5DataType
            {
                Id = _typeId,
                Size = typeSize,
            };

            if (typeOrder == H5T.order_t.BE)
            {
                dt.ByteOrder = Hdf5ByteOrder.BigEndian;
            }
            else if (typeOrder == H5T.order_t.LE)
            {
                dt.ByteOrder = Hdf5ByteOrder.LittleEndian;
            }
            else if (typeOrder == H5T.order_t.ONE)
            {
                dt.ByteOrder = Hdf5ByteOrder.None;
            }

            if (typeClass == H5T.class_t.INTEGER)
            {
                if (typeSign == H5T.sign_t.NONE)
                {
                    if (typeSize == 1)
                    {
                        dt.Type = Hdf5DataTypes.UInt8;
                        dt.NativeType = H5T.NATIVE_UINT8.ToId();
                    }
                    else if (typeSize == 2)
                    {
                        dt.Type = Hdf5DataTypes.UInt16;
                        dt.NativeType = H5T.NATIVE_UINT16.ToId();
                    }
                    else if (typeSize == 4)
                    {
                        dt.Type = Hdf5DataTypes.UInt32;
                        dt.NativeType = H5T.NATIVE_UINT32.ToId();
                    }
                    else if (typeSize == 8)
                    {
                        dt.Type = Hdf5DataTypes.UInt64;
                        dt.NativeType = H5T.NATIVE_UINT64.ToId();
                    }
                }
                else
                {
                    if (typeSize == 1)
                    {
                        dt.Type = Hdf5DataTypes.Int8;
                        dt.NativeType = H5T.NATIVE_INT8.ToId();
                    }
                    else if (typeSize == 2)
                    {
                        dt.Type = Hdf5DataTypes.Int16;
                        dt.NativeType = H5T.NATIVE_INT16.ToId();
                    }
                    else if (typeSize == 4)
                    {
                        dt.Type = Hdf5DataTypes.Int32;
                        dt.NativeType = H5T.NATIVE_INT32.ToId();
                    }
                    else if (typeSize == 8)
                    {
                        dt.Type = Hdf5DataTypes.Int64;
                        dt.NativeType = H5T.NATIVE_INT64.ToId();
                    }
                }
            }
            else if (typeClass == H5T.class_t.FLOAT)
            {
                if (typeSize == 4)
                {
                    dt.Type = Hdf5DataTypes.Single;
                    dt.NativeType = H5T.NATIVE_FLOAT.ToId();
                }
                else if (typeSize == 8)
                {
                    dt.Type = Hdf5DataTypes.Double;
                    dt.NativeType = H5T.NATIVE_DOUBLE.ToId();
                }
            }
            else if (typeClass == H5T.class_t.STRING)
            {
                dt.Type = Hdf5DataTypes.String;
                dt.NativeType = H5T.C_S1.ToId();
            }

            return dt;
        }

        /// <summary>
        /// Returns the datatype.  
        /// This assumes that the object is already open
        /// </summary>
        /// <param name="_objectId"></param>
        /// <returns></returns>
        public static Hdf5DataType GetDataType(Hdf5Identifier _objectId)
        {
            var typeId = H5D.get_type(_objectId.Value).ToId();

            if (typeId.Value > 0)
            {
                return GetDataTypeByType(typeId);
            }

            return null;
        }

        public static Hdf5Identifier GetNativeType(Hdf5DataTypes _datatype)
        {
            switch (_datatype)
            {
                case Hdf5DataTypes.UInt8:
                    return H5T.NATIVE_UINT8.ToId();
                case Hdf5DataTypes.UInt16:
                    return H5T.NATIVE_UINT16.ToId();
                case Hdf5DataTypes.UInt32:
                    return H5T.NATIVE_UINT32.ToId();
                case Hdf5DataTypes.UInt64:
                    return H5T.NATIVE_UINT64.ToId();
                case Hdf5DataTypes.Int8:
                    return H5T.NATIVE_INT8.ToId();
                case Hdf5DataTypes.Int16:
                    return H5T.NATIVE_INT16.ToId();
                case Hdf5DataTypes.Int32:
                    return H5T.NATIVE_INT32.ToId();
                case Hdf5DataTypes.Int64:
                    return H5T.NATIVE_INT64.ToId();
                case Hdf5DataTypes.Single:
                    return H5T.NATIVE_FLOAT.ToId();
                case Hdf5DataTypes.Double:
                    return H5T.NATIVE_DOUBLE.ToId();
                case Hdf5DataTypes.String:
                    return H5T.C_S1.ToId();
            }

            throw new ArgumentOutOfRangeException("_datatype", "Unknown type");
        }

        public static Hdf5DataTypes GetDataTypesEnum<T>(T _value)
        {
            Type t = typeof(T);

            if (t == typeof(byte))
            {
                return Hdf5DataTypes.UInt8;
            }

            if (t == typeof (UInt16))
            {
                return Hdf5DataTypes.UInt16;                
            }

            if (t == typeof (UInt32))
            {
                return Hdf5DataTypes.Int32;
            }

            if (t == typeof (UInt64))
            {
                return Hdf5DataTypes.UInt64;
            }

            if (t == typeof (sbyte))
            {
                return Hdf5DataTypes.Int8;
            }

            if (t == typeof (Int16))
            {
                return Hdf5DataTypes.Int16;
            }

            if (t == typeof (Int32))
            {
                return Hdf5DataTypes.Int32;
            }

            if (t == typeof (Int64))
            {
                return Hdf5DataTypes.Int64;
            }

            if (t == typeof (Single))
            {
                return Hdf5DataTypes.Single;
            }

            if (t == typeof (Double))
            {
                return Hdf5DataTypes.Double;
            }

            if (t == typeof (string))
            {
                return Hdf5DataTypes.String;
            }

            throw new Hdf5UnsupportedDataTypeException();
        }
    }
}

