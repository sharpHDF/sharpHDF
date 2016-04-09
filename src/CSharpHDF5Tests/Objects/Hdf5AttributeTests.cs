using CSharpHDF5.Objects;
using NUnit.Framework;

namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5AttributeTests
    {

        [Test]
        public void GetAttributes()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            ReadonlyList<Hdf5Attribute> attributes = file.Attributes;

            file.Close();
        }
    }
}
