using System.IO;
using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5GroupTests
    {
        private string m_Directory = @"c:\temp\hdf5tests\grouptests";

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
        public void CreateGroup()
        {
            string fileName = GetFilename("creategroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");

            Assert.IsNotNull(group);
            Assert.AreEqual(1, file.Groups.Count);

            file.Close();
        }

        [Test]
        public void OpenGroup()
        {
            string fileName = GetFilename("opengroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");

            Assert.IsNotNull(group);
            Assert.AreEqual(1, file.Groups.Count);

            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];

            Assert.IsNotNull(group);
        }

        [Test]
        public void CreateGroupInGroup()
        {
            string fileName = GetFilename("creategroupingroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group1 = file.AddGroup("group1");

            Assert.IsNotNull(group1);
            Assert.AreEqual(1, file.Groups.Count);

            Hdf5Group group2 = group1.AddGroup("group2");
            Assert.IsNotNull(group2);
            Assert.AreEqual(1, file.Groups.Count);
            Assert.AreEqual(1, group1.Groups.Count);

            file.Close();
        }

        [Test]
        public void OpeningGroupInGroup()
        {
            string fileName = GetFilename("opengroupingroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group1 = file.AddGroup("group1");

            Assert.IsNotNull(group1);
            Assert.AreEqual(1, file.Groups.Count);

            Hdf5Group group2 = group1.AddGroup("group2");
            Assert.AreEqual(1, file.Groups.Count);
            Assert.AreEqual(1, group1.Groups.Count);

            file.Close();

            file = new Hdf5File(fileName);
            group1 = file.Groups[0];
            Assert.IsNotNull(group1);
            Assert.AreEqual("group1", group1.Name);

            group2 = group1.Groups[0];
            Assert.IsNotNull(group2);
            Assert.AreEqual("group2", group2.Name);
        }

    }
}
