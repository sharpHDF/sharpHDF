/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using NUnit.Framework;
using sharpHDF.Library.Exceptions;
using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Tests.Objects
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
                Hdf5File file = Hdf5File.Create(filename);
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

            Hdf5File file = Hdf5File.Create(filename);

            Assert.IsNotNull(file);

            file.Close();
        }

        [Test]
        public void OpenFile()
        {
            string filename = GetFilename("openfile.h5");
            Hdf5File file = Hdf5File.Create(filename);
            Assert.IsNotNull(file);
            file.Close();

            file = new Hdf5File(filename);
            Assert.IsNotNull(file);

            file.Dispose();
        }

        [Test]
        public void OpenFileWithException()
        {
            string filename = GetFilename("openfilewithexception.h5");

            Hdf5File file = Hdf5File.Create(filename);

            Assert.IsNotNull(file);

            file.Close();

            //Lock the file so can't be opened
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                try
                {
                    //Try to open it
                    file = new Hdf5File(filename);

                    Assert.Fail("Should have thrown an exception");
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOf<Hdf5UnknownException>(ex);
                }

                fs.Close();
            }
        }

        [Test]
        public void CreateFileWithException()
        {
            //Drive that doesn't exist
            string filename = @"q:\createfilewithexception.h5";

            try
            {
                Hdf5File file = Hdf5File.Create(filename);
                file.Close();
                Assert.Fail("Should have caused an exception");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5UnknownException>(ex);
            }
        }
    }
}
