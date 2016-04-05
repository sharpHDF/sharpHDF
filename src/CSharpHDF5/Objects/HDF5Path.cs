using System.Text;

namespace CSharpHDF5.Objects
{
    public class Hdf5Path
    {
        public Hdf5Path(string _fullPath)
        {
            FullPath = _fullPath;

            string[] parts = _fullPath.Split("/".ToCharArray());
            if (parts.Length > 0)
            {
                Name = parts[parts.Length - 1];
            }

            if (Name == ".")
            {
                FullPath = "/";
                ParentPath = "/";
            }
            else if (parts.Length == 1)
            {
                ParentPath = "/";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    sb.Append(parts[0]);
                    sb.Append("/");
                }

                ParentPath = sb.ToString();
            }
        }

        public string Combine(string _child)
        {
            if (FullPath == "/")
            {
                return _child;
            }

            return FullPath + "/" + _child;
        }

        public string FullPath { get; private set; }
        public string ParentPath { get; private set; }
        public string Name { get; private set; }
    }
}
