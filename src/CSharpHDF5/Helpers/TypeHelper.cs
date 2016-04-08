using System;
using CSharpHDF5.Enums;
using CSharpHDF5.Structs;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    public static class TypeHelper
    {

        /// <summary>
        /// Returns the datatype.  
        /// This assumes that the object is already open
        /// </summary>
        /// <param name="_objectId"></param>
        /// <returns></returns>
        public static Hdf5DataType GetDataType(Hdf5Identifier _objectId)
        {
            var typeId = H5D.get_type(_objectId.Value).ToId();
            var typeNative = H5T.get_native_type(typeId.Value, H5T.direction_t.DEFAULT).ToId();           
            var typeClass = H5T.get_class(typeId.Value);
            var typeSize = (int) H5T.get_size(typeId.Value);

            var typeSign = H5T.get_sign(typeId.Value);
            var typeOrder = H5T.get_order(typeId.Value);

            Hdf5DataType dt = new Hdf5DataType
            {
                Id = typeId,
                Size = typeSize
            };

            if (typeOrder == H5T.order_t.BE)
            {
                dt.ByteOrder = Hdf5ByteOrder.BigEndian;
            }
            else if (typeOrder == H5T.order_t.LE)
            {
                dt.ByteOrder = Hdf5ByteOrder.LittleEndian;
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

            H5T.close(typeId.Value);

            return dt;
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
            }

            throw new ArgumentOutOfRangeException("_datatype", "Unknown type");
        }
    }
}

