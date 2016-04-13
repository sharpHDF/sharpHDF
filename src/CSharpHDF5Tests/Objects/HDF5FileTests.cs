using System;
using System.IO;
using CSharpHDF5.Exceptions;
using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5FileTests : BaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DirectoryName = @"c:\temp\hdf5tests\filetests";

            CleanDirectory();
        }

        [Test]
        public void CreateAttemptExistingFile()
        {
            string filename = GetFilename("existingfile.h5");
            File.WriteAllText(filename, "");

            try
            {
                Hdf5File file = Hdf5File.CreateFile(filename);
                file.Close();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5FileExistsException>(ex);
                return;
            }

            Assert.Fail("Should have caused an exception");
        }

        [Test]
        public void OpenAttemptMissingFile()
        {
            string filename = GetFilename("missingfile.h5");

            try
            {
                Hdf5File file = new Hdf5File(filename);
                file.Close();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5FileNotFoundException>(ex);
                return;
            }

            Assert.Fail("Should have caused an exception");
        }

        [Test]
        public void CreateFile()
        {
            string filename = GetFilename("createfile.h5");

            Hdf5File file = Hdf5File.CreateFile(filename);

            Assert.IsNotNull(file);

            file.Close();
        }

        [Test]
        public void OpenFile()
        {
            string filename = GetFilename("openfile.h5");
            Hdf5File file = Hdf5File.CreateFile(filename);
            Assert.IsNotNull(file);
            file.Close();

            file = new Hdf5File(filename);
            Assert.IsNotNull(file);
        }
    }
}
