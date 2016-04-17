/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;

namespace sharpHDF.Library.Objects
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
                    sb.Append(parts[i]);
                    sb.Append("/");
                }

                if (sb.Length > 2)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                ParentPath = sb.ToString();
            }
        }


        public string FullPath { get; private set; }
        public string ParentPath { get; private set; }
        public string Name { get; private set; }
    }
}
