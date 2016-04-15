using System;
using System.Collections.Generic;
using CSharpHDF5.Enums;
using CSharpHDF5.Exceptions;
using CSharpHDF5.Objects;
using NUnit.Framework;


namespace CSharpHDF5Tests.Objects
{
    [TestFixture]
    public class Hdf5DatasetTests : BaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DirectoryName = @"c:\temp\hdf5tests\datasettests";

            CleanDirectory();
        }

        [Test]
        public void CreateDatasetInFile()
        {
            string filename = GetFilename("createdatasetinfile.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty {CurrentSize = 100 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);
            Assert.AreEqual(1, file.Datasets.Count);

            file.Close();
        }

        [Test]
        public void OpenDatasetInFile()
        {
            string filename = GetFilename("opendatasetinfile.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 100 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);
            Assert.AreEqual(1, file.Datasets.Count);
            file.Close();

            file = new Hdf5File(filename);
            dataset = file.Datasets[0];
            Assert.IsNotNull(dataset);
            Assert.AreEqual("dataset1", dataset.Name);
            Assert.AreEqual(Hdf5DataTypes.Int8, dataset.DataType.Type);
            file.Close();
        }

        [Test]
        public void CreateDatasetInGroup()
        {
            string filename = GetFilename("createdatasetingroup.h5");

            Hdf5File file = Hdf5File.Create(filename);

            Hdf5Group group = file.Groups.Add("group1");

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 100 };
            properties.Add(property);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);
            Assert.AreEqual(0, file.Datasets.Count);
            Assert.AreEqual(1, group.Datasets.Count);

            file.Close();
        }

        [Test]
        public void OpenDatasetInGroup()
        {
            string filename = GetFilename("opendatasetingroup.h5");

            Hdf5File file = Hdf5File.Create(filename);

            Hdf5Group group = file.Groups.Add("group1");

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 100 };
            properties.Add(property);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);
            Assert.AreEqual(0, file.Datasets.Count);
            Assert.AreEqual(1, group.Datasets.Count);

            file.Close();

            file = new Hdf5File(filename);
            group = file.Groups[0];
            dataset = group.Datasets[0];

            Assert.IsNotNull(dataset);
            Assert.AreEqual("dataset1", dataset.Name);
        }

        [Test]
        public void SetDataNull()
        {
            string filename = GetFilename("setdatatnull.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 100 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);

            try
            {
                sbyte[] value = null;
                dataset.SetData(value);
                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ArgumentNullException>(ex);
            }
            
            file.Close();
        }

        [Test]
        public void SetDataMismatch()
        {
            string filename = GetFilename("setdatamismatch.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 3 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);

            try
            {
                Int32[] value = {1, 2, 3};
                dataset.SetData(value);
                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5TypeMismatchException>(ex);
            }

            file.Close();
        }

        [Test]
        public void SetArraySizeMismatch()
        {
            string filename = GetFilename("setarraysizemismatch.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 3 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, 1, properties);
            Assert.IsNotNull(dataset);

            try
            {
                Int32[] value = { 1, 2, 3, 4, 5 };
                dataset.SetData(value);
                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5ArraySizeMismatchException>(ex);
            }

            file.Close();
        }

        [Test]
        public void SetInt8()
        {
            string filename = GetFilename("setint8.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, 1, properties);
            Assert.IsNotNull(dataset);

            SByte[] value = { SByte.MinValue, -1, 0, 1, SByte.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetInt16()
        {
            string filename = GetFilename("setint16.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int16, 1, properties);
            Assert.IsNotNull(dataset);

            Int16[] value = { Int16.MinValue, -1, 0, 1, Int16.MaxValue };
            dataset.SetData(value);

            file.Close();
        }

        [Test]
        public void SetInt32()
        {
            string filename = GetFilename("setint32.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, 1, properties);
            Assert.IsNotNull(dataset);

            Int32[] value = { Int32.MinValue, -1, 0, 1, Int32.MaxValue };
            dataset.SetData(value);

            file.Close();
        }

        [Test]
        public void SetInt64()
        {
            string filename = GetFilename("setint64.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int64, 1, properties);
            Assert.IsNotNull(dataset);

            Int64[] value = { Int64.MinValue, -1, 0, 1, Int64.MaxValue };
            dataset.SetData(value);

            file.Close();
        }

        [Test]
        public void SetUInt8()
        {
            string filename = GetFilename("setuint8.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt8, 1, properties);
            Assert.IsNotNull(dataset);

            Byte[] value = { Byte.MinValue, 1, 2, 3, Byte.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetUInt16()
        {
            string filename = GetFilename("setuint16.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt16, 1, properties);
            Assert.IsNotNull(dataset);

            UInt16[] value = { UInt16.MinValue, 1, 2, 3, UInt16.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetUInt32()
        {
            string filename = GetFilename("setuint32.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt32, 1, properties);
            Assert.IsNotNull(dataset);

            UInt32[] value = { UInt32.MinValue, 1, 2, 3, UInt32.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetUInt64()
        {
            string filename = GetFilename("setuint64.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt64, 1, properties);
            Assert.IsNotNull(dataset);

            UInt64[] value = { UInt64.MinValue, 1, 2, 3, UInt64.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetSingle()
        {
            string filename = GetFilename("setsingle.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Single, 1, properties);
            Assert.IsNotNull(dataset);

            Single[] value = { Single.MinValue, -1.1f, 0.0f, -1.3f, Single.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        [Test]
        public void SetDouble()
        {
            string filename = GetFilename("setdouble.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Double, 1, properties);
            Assert.IsNotNull(dataset);

            Double[] value = { Double.MinValue, -1.1d, 0.0d, -1.3d, Double.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            file.Close();
        }

        //[Test]
        //public void ReadDataset1DInt8()
        //{
        //    string fileName = @"c:\temp\test.h5";

        //    Hdf5File file = new Hdf5File(fileName);

        //    Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[1];

        //    Array array = dataset.GetData();

        //    file.Close();
        //}

        //[Test]
        //public void ReadDataset2DInt8()
        //{
        //    string fileName = @"c:\temp\test.h5";

        //    Hdf5File file = new Hdf5File(fileName);

        //    Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[7];

        //    Array array = dataset.GetData();

        //    file.Close();
        //}


        //[Test]
        //public void ReadDataset2DInt16()
        //{
        //    string fileName = @"c:\temp\test.h5";

        //    Hdf5File file = new Hdf5File(fileName);

        //    Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[12];

        //    Array array = dataset.GetData();

        //    file.Close();
        //}

        //[Test]
        //public void ReadDataset1DSingle()
        //{
        //    string fileName = @"c:\temp\test.h5";

        //    Hdf5File file = new Hdf5File(fileName);

        //    Hdf5Dataset dataset = file.Groups[0].Groups[0].Datasets[13];

        //    Array array = dataset.GetData();

        //    file.Close();
        //}

    }
}
