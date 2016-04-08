using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpHDF5.Objects;
using NUnit.Framework;


namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5DatasetTests
    {
        [Test]
        public void ReadDataset1DInt8()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[1];

            Array array = dataset.GetData();

            file.Close();
        }

        [Test]
        public void ReadDataset2DInt8()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[7];

            Array array = dataset.GetData();

            file.Close();
        }


        [Test]
        public void ReadDataset2DInt16()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[12];

            Array array = dataset.GetData();

            file.Close();
        }

        [Test]
        public void ReadDataset1DSingle()
        {
            string fileName = @"c:\temp\test.h5";

            Hdf5File file = new Hdf5File(fileName);

            Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[13];

            Array array = dataset.GetData();

            file.Close();
        }

    }
}
