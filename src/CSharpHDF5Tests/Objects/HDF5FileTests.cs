using System;
using System.Collections.Generic;
using System.IO;
using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5FileTests
    {
        [Test]
        public void Create()
        {
            string fileName = @"c:\temp\fileCreate.h5";
            Hdf5File file = new Hdf5File(fileName);

            Assert.IsTrue(File.Exists(fileName));

            file.Close();
        }

        [Test]
        public void Open()
        {
            string fileName = @"c:\temp\fileCreate.h5";

            Hdf5File file = new Hdf5File(fileName);
            file.Close();

            Assert.IsTrue(File.Exists(fileName));

            file = new Hdf5File(fileName);
            file.Close();

        }

        [Test]
        public void GetGroups()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            List<Hdf5Group> groups = file.Groups;

            file.Close();
        }

        [Test]
        public void GetAttributes()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            List<Hdf5Attribute> attributes = file.Attributes;

            file.Close();
        }
    }
}
