using System.IO;

namespace CSharpHDF5Tests.Objects
{
    public abstract class BaseTest
    {
        protected string DirectoryName { get; set; }
        
        public void CleanDirectory()
        {
            if (!Directory.Exists(DirectoryName))
            {
                Directory.CreateDirectory(DirectoryName);
            }

            string[] files = Directory.GetFiles(DirectoryName);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        protected string GetFilename(string _file)
        {
            return Path.Combine(DirectoryName, _file);
        }
    }
}
