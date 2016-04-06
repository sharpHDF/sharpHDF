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
                        dt.Type= Hdf5DataTypes.Int8;                        
                    }
                    else if (typeSize == 2)
                    {
                        dt.Type = Hdf5DataTypes.Int16;  
                    }
                    else if (typeSize == 4)
                    {
                        dt.Type = Hdf5DataTypes.Int32;  
                    }
                    else if (typeSize == 8)
                    {
                        dt.Type = Hdf5DataTypes.Int64;  
                    }
                }
                else
                {
                    if (typeSize == 1)
                    {
                        dt.Type = Hdf5DataTypes.UInt8;
                    }
                    else if (typeSize == 2)
                    {
                        dt.Type = Hdf5DataTypes.UInt16;
                    }
                    else if (typeSize == 4)
                    {
                        dt.Type = Hdf5DataTypes.UInt32;
                    }
                    else if (typeSize == 8)
                    {
                        dt.Type = Hdf5DataTypes.UInt64;
                    }
                }
            }

            H5T.close(typeId.Value);

            return dt;
        }
    }
}

