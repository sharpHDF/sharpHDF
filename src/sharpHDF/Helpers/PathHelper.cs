using System;
using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Helpers
{
    public static class PathHelper
    {
        public static Hdf5Path Append(this Hdf5Path _path, string _childName)
        {
            if (_childName == null)
            {
                throw new ArgumentNullException("_childName");
            }

            if (_path != null
                && _path.FullPath!="/")
            {
                string fullPath = _path.FullPath;

                if (fullPath.EndsWith("/"))
                {
                    return new Hdf5Path(fullPath + _childName);
                }

                return new Hdf5Path(fullPath + "/" + _childName);
            }
            
            return new Hdf5Path(_childName);
        }
    }
}
