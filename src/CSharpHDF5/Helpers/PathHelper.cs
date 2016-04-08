using System.IO;
using CSharpHDF5.Objects;

namespace CSharpHDF5.Helpers
{
    public static class PathHelper
    {
        public static Hdf5Path Append(this Hdf5Path _path, string _childName)
        {
            string temp = Path.Combine(_path.FullPath, _childName);

            Hdf5Path path = new Hdf5Path(temp);

            return path;
        }
    }
}
