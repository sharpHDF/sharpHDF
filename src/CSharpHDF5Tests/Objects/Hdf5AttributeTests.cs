using System.IO;
using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5AttributeTests
    {
        private string m_Directory = @"c:\temp\hdf5tests\attributetests";

        [OneTimeSetUp]
        public void Setup()
        {
            if (!Directory.Exists(m_Directory))
            {
                Directory.CreateDirectory(m_Directory);
            }

            string[] files = Directory.GetFiles(m_Directory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private string GetFilename(string _file)
        {
            return Path.Combine(m_Directory, _file);
        }

        [Test]
        public void CreateAttribute()
        {
            string fileName = GetFilename("createattribute.h5");

            Hdf5File file = new Hdf5File(fileName);
            //file.a

            file.Close();
        }


        [Test]
        public void GetAttributes()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            ReadonlyList<Hdf5Attribute> attributes = file.Attributes;

            attributes = file.Groups[0].Attributes;

            file.Close();
        }
    }
}
