using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpHDF5.Objects;
using HDF.PInvoke;

namespace CSharpHDF5.Helpers
{
    public static class GroupHelper
    {
        public static int GetCount(int _identifier)
        {
            ulong pos = 0;
            int id = 0;
            bool done = false;

            int count = 0;

            do
            {
                H5G.info_t info = new H5G.info_t();
                string groupName = ".";

                id = H5G.get_info_by_idx(_identifier, groupName, H5.index_t.NAME, H5.iter_order_t.NATIVE, pos, ref info);

                if (id > 0)
                {
                    count ++;
                }
                else
                {
                    done = true;
                }

            } while (!done);

            return count;
        }


        public static  List<Hdf5Group> GetGroups(int _identifier)
        {
            ulong pos = 0;
            int id = 0;
            bool done = false;

            List<Hdf5Group> groups = new List<Hdf5Group>();

            List<string> datasetNames = new List<string>();
            List<string> groupNames = new List<string>();
            var rootId = H5G.open(_identifier, "/");

            H5O.visit(_identifier, H5.index_t.NAME, H5.iter_order_t.INC, 
                delegate(int _objectId, IntPtr _namePtr, ref H5O.info_t _info, IntPtr _opData)
            {
                string objectName = Marshal.PtrToStringAnsi(_namePtr);
                H5O.info_t gInfo = new H5O.info_t();
                H5O.get_info_by_name(_objectId, objectName, ref gInfo);

                if (gInfo.type == H5O.type_t.DATASET)
                {
                    datasetNames.Add(objectName);
                }
                else if (gInfo.type == H5O.type_t.GROUP)
                {
                    groupNames.Add(objectName);
                }
                return 0;
            }, new IntPtr());

            H5G.close(rootId);


            //do
            //{
            //    H5G.info_t info = new H5G.info_t();
            //    string groupName = ".";

            //    id = H5G.get_info_by_idx(_identifier, groupName, H5.index_t.NAME, H5.iter_order_t.NATIVE, pos, ref info);

            //    if (id >= 0)
            //    {
            //        int groupId = H5G.open(_identifier, ".");

            //        pos++;
            //    }
            //    else
            //    {
            //        done = true;
            //    }

            //} while (!done);

            return groups;
        }
    }
}
