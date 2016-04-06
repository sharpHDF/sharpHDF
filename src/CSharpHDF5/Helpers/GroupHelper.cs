using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSharpHDF5.Interfaces;
using CSharpHDF5.Objects;
using CSharpHDF5.Struct;
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

        //public static List<Hdf5Group> GetGroupsBelowObject(int _identifier)
        //{
        //    ulong pos = 0;
        //    int id = 0;
        //    bool done = false;

        //    List<Hdf5Group> groups = new List<Hdf5Group>();
        //    List<Hdf5Dataset> datasets = new List<Hdf5Dataset>();

        //    var rootId = H5G.open(_identifier, "/");

        //    H5O.visit(_identifier, H5.index_t.NAME, H5.iter_order_t.INC, 
        //        delegate(int _objectId, IntPtr _namePtr, ref H5O.info_t _info, IntPtr _opData)
        //    {
        //        string objectName = Marshal.PtrToStringAnsi(_namePtr);
        //        H5O.info_t gInfo = new H5O.info_t();
        //        H5O.get_info_by_name(_objectId, objectName, ref gInfo);

        //        int oid = H5O.open(_objectId, objectName);
        //        H5O.close(oid);

        //        if (gInfo.type == H5O.type_t.DATASET)
        //        {
        //            Hdf5Dataset dataset = new Hdf5Dataset(_objectId, objectName);
        //            datasets.Add(dataset);
        //        }
        //        else if (gInfo.type == H5O.type_t.GROUP)
        //        {
        //            Hdf5Group group = new Hdf5Group(_objectId, objectName);
        //            groups.Add(group);
        //        }

        //        return 0;
        //    }, new IntPtr());

        //    H5G.close(rootId);

        //    return groups;
        //}

        public static void PopulateChildrenObjects<T>(Hdf5Identifier _fileId, T _parentObject)  where T : AbstractHdf5Object
        {
            ulong pos = 0;

            List<string> groupNames = new List<string>();

            var id = H5G.open(_fileId.Value, _parentObject.Path.FullPath).ToId();

            if (id.Value > 0)
            {
                ArrayList al = new ArrayList();
                GCHandle hnd = GCHandle.Alloc(al);
                IntPtr op_data = (IntPtr) hnd;

                H5L.iterate(_parentObject.Id.Value, H5.index_t.NAME, H5.iter_order_t.NATIVE, ref pos,
                    delegate(int _objectId, IntPtr _namePtr, ref H5L.info_t _info, IntPtr _data)
                    {
                        string objectName = Marshal.PtrToStringAnsi(_namePtr);

                        groupNames.Add(objectName);

                        return 0;
                    }, op_data);

                hnd.Free();

                H5G.close(id.Value);

                foreach (var groupName in groupNames)
                {
                    Object hdf5Obj = GetObject(_fileId, _parentObject, groupName);

                    if (hdf5Obj != null)
                    {
                        if (hdf5Obj is Hdf5Dataset)
                        {
                            var parent = _parentObject as IHasDatasets;

                            if (parent != null)
                            {
                                parent.Datasets.Add(hdf5Obj as Hdf5Dataset);
                            }
                        }
                        else if (hdf5Obj is Hdf5Group)
                        {
                            var parent = _parentObject as IHasGroups;

                            if (parent != null)
                            {
                                parent.Groups.Add(hdf5Obj as Hdf5Group);
                            }
                        }
                    }
                }
            }
        }

        public static object GetObject(Hdf5Identifier _fileId, AbstractHdf5Object _parent, string _objectName)
        {
            string fullPath = _parent.Path.Combine(_objectName);

            H5O.info_t gInfo = new H5O.info_t();
            H5O.get_info_by_name(_fileId.Value, fullPath, ref gInfo);

            var id = H5O.open(_fileId.Value, fullPath).ToId();
            if (id.Value > 0)
            {
                H5O.close(id.Value);

                if (gInfo.type == H5O.type_t.DATASET)
                {
                    Hdf5Dataset dataset = new Hdf5Dataset(id, fullPath);
                    return dataset;
                }

                if (gInfo.type == H5O.type_t.GROUP)
                {
                    Hdf5Group group = new Hdf5Group(_fileId, id, fullPath);
                    group.LoadChildObjects();
                    return group;
                }
            }

            return null;
        }
    }
}
