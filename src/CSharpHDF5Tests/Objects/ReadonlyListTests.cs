using System.Collections;
using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class ReadonlyListTests : BaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DirectoryName = @"c:\temp\hdf5tests\readonlylist";

            CleanDirectory();
        }

        [Test]
        public void ContainsTest()
        {
            string fileName = GetFilename("contains.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group1 = file.Groups.Add("group1");
            Hdf5Group group2 = file.Groups.Add("group2");
            Hdf5Group group3 = file.Groups.Add("group3");
            Hdf5Group group4 = file.Groups.Add("group4");
            Hdf5Group group5 = file.Groups.Add("group5");

            Assert.IsTrue(file.Groups.Contains(group1));
            Assert.IsTrue(file.Groups.Contains(group2));
            Assert.IsTrue(file.Groups.Contains(group3));
            Assert.IsTrue(file.Groups.Contains(group4));
            Assert.IsTrue(file.Groups.Contains(group5));
        }

        [Test]
        public void CopyToTest()
        {
            string fileName = GetFilename("copyto.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group1 = file.Groups.Add("group1");
            Hdf5Group group2 = file.Groups.Add("group2");
            Hdf5Group group3 = file.Groups.Add("group3");
            Hdf5Group group4 = file.Groups.Add("group4");
            Hdf5Group group5 = file.Groups.Add("group5");

            Hdf5Group[] groups = new Hdf5Group[5];
            file.Groups.CopyTo(groups, 0);
        }

        [Test]
        public void GetEnumeratorTest()
        {
            string fileName = GetFilename("getenumerator.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group1 = file.Groups.Add("group1");
            Hdf5Group group2 = file.Groups.Add("group2");
            Hdf5Group group3 = file.Groups.Add("group3");
            Hdf5Group group4 = file.Groups.Add("group4");
            Hdf5Group group5 = file.Groups.Add("group5");

            var groups = file.Groups;
            var test = groups as IEnumerable;
            int i = 0;
            foreach (object testGroup in test)
            {
                i++;
            }

            Assert.AreEqual(5, i);
        }

    }
}
