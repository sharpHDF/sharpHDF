using System.Collections.Generic;
using System.IO;
using CSharpHDF5.Enums;
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
        public void CreateAttributeOnFile()
        {
            string fileName = GetFilename("createattributeonfile.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            file.AddAttribute("attribute1", "test");
            file.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, file.Attributes.Count);

            file.Close();
        }

        [Test]
        public void GetAttributeOnFile()
        {
            string fileName = GetFilename("getattributeonfile.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            file.AddAttribute("attribute1", "test");
            file.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, file.Attributes.Count);

            file.Close();

            file = new Hdf5File(fileName);
            var attibutes = file.Attributes;
            Assert.AreEqual(2, attibutes.Count);

            var attribute1 = attibutes[0];
            Assert.AreEqual("attribute1", attribute1.Name);
            Assert.AreEqual("test", attribute1.Value);

            var attribute2 = attibutes[1];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }

        [Test]
        public void CreateAttributeOnGroup()
        {
            string fileName = GetFilename("createattributeongroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");
            group.AddAttribute("attribute1", "test");
            group.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, group.Attributes.Count);

            file.Close();
        }

        [Test]
        public void GetAttributeOnGroup()
        {
            string fileName = GetFilename("getattributeongroup.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");
            group.AddAttribute("attribute1", "test");
            group.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, group.Attributes.Count);

            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            var attibutes = group.Attributes;
            Assert.AreEqual(2, attibutes.Count);

            var attribute1 = attibutes[0];
            Assert.AreEqual("attribute1", attribute1.Name);
            Assert.AreEqual("test", attribute1.Value);

            var attribute2 = attibutes[1];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }

        [Test]
        public void CreateAttributeOnDataset()
        {
            string fileName = GetFilename("createattributeondataset.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty {CurrentSize = 1};
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.AddDataset("dataset1", Hdf5DataTypes.Int32, 1, dimensionProps);
            dataset.AddAttribute("attribute1", "test");
            dataset.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, dataset.Attributes.Count);

            file.Close();
        }

        [Test]
        public void GetAttributeOnDataset()
        {
            string fileName = GetFilename("createattributeondataset.h5");

            Hdf5File file = Hdf5File.CreateFile(fileName);
            Hdf5Group group = file.AddGroup("group1");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.AddDataset("dataset1", Hdf5DataTypes.Int32, 1, dimensionProps);
            dataset.AddAttribute("attribute1", "test");
            dataset.AddAttribute("attribute2", 5);
            Assert.AreEqual(2, dataset.Attributes.Count);

            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            dataset = group.Datasets[0];
            var attibutes = dataset.Attributes;
            Assert.AreEqual(2, attibutes.Count);

            var attribute1 = attibutes[0];
            Assert.AreEqual("attribute1", attribute1.Name);
            Assert.AreEqual("test", attribute1.Value);

            var attribute2 = attibutes[1];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }


  
    }
}
