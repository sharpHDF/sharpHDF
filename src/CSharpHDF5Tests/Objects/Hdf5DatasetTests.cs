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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
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
        public void Set2DArraySizeMismatch1()
        {
            string filename = GetFilename("set2darraysizemismatch1.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property1 = new Hdf5DimensionProperty { CurrentSize = 3 };
            properties.Add(property1);
            Hdf5DimensionProperty property2 = new Hdf5DimensionProperty { CurrentSize = 2 };
            properties.Add(property2);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
            Assert.IsNotNull(dataset);

            try
            {
                //5x2 rather than 3x2
                Int32[,] value = {{ 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 }};
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
        public void Set2DArraySizeMismatch2()
        {
            string filename = GetFilename("set2darraysizemismatch2.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property1 = new Hdf5DimensionProperty { CurrentSize = 3 };
            properties.Add(property1);
            Hdf5DimensionProperty property2 = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property2);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
            Assert.IsNotNull(dataset);

            try
            {
                //Only 3x4
                Int32[,] value = { { 1, 2, 3 }, { 4, 5, 6, }, { 7, 8, 9, }, { 10, 11, 12, } };
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
        public void SetArrayDimensionMismatch1()
        {
            string filename = GetFilename("setarraydimensionmismatch1.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
            Assert.IsNotNull(dataset);

            try
            {
                //2 dimensions sent, only one expected
                Int32[,] value = { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 } };
                dataset.SetData(value);
                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5ArrayDimensionsMismatchException>(ex);
            }

            file.Close();
        }

        [Test]
        public void SetArrayDimensionMismatch2()
        {
            string filename = GetFilename("setarraydimensionmismatch2.h5");

            Hdf5File file = Hdf5File.Create(filename);

            List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty property1 = new Hdf5DimensionProperty { CurrentSize = 5 };
            properties.Add(property1);
            Hdf5DimensionProperty property2 = new Hdf5DimensionProperty { CurrentSize = 2 };
            properties.Add(property2);

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
            Assert.IsNotNull(dataset);

            try
            {
                //1 dimensions sent, 2 expected
                Int32[] value = {  1, 2, 3, 4, 5 } ;
                dataset.SetData(value);
                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5ArrayDimensionsMismatchException>(ex);
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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
            Assert.IsNotNull(dataset);

            SByte[] value = { SByte.MinValue, -1, 0, 1, SByte.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(SByte.MinValue, array.GetValue(0));
            Assert.AreEqual(SByte.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int16, properties);
            Assert.IsNotNull(dataset);

            Int16[] value = { Int16.MinValue, -1, 0, 1, Int16.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Int16.MinValue, array.GetValue(0));
            Assert.AreEqual(Int16.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int32, properties);
            Assert.IsNotNull(dataset);

            Int32[] value = { Int32.MinValue, -1, 0, 1, Int32.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Int32.MinValue, array.GetValue(0));
            Assert.AreEqual(Int32.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Int64, properties);
            Assert.IsNotNull(dataset);

            Int64[] value = { Int64.MinValue, -1, 0, 1, Int64.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Int64.MinValue, array.GetValue(0));
            Assert.AreEqual(Int64.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt8, properties);
            Assert.IsNotNull(dataset);

            Byte[] value = { Byte.MinValue, 1, 2, 3, Byte.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Byte.MinValue, array.GetValue(0));
            Assert.AreEqual(Byte.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt16, properties);
            Assert.IsNotNull(dataset);

            UInt16[] value = { UInt16.MinValue, 1, 2, 3, UInt16.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(UInt16.MinValue, array.GetValue(0));
            Assert.AreEqual(UInt16.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt32, properties);
            Assert.IsNotNull(dataset);

            UInt32[] value = { UInt32.MinValue, 1, 2, 3, UInt32.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(UInt32.MinValue, array.GetValue(0));
            Assert.AreEqual(UInt32.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.UInt64, properties);
            Assert.IsNotNull(dataset);

            UInt64[] value = { UInt64.MinValue, 1, 2, 3, UInt64.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(UInt64.MinValue, array.GetValue(0));
            Assert.AreEqual(UInt64.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Single, properties);
            Assert.IsNotNull(dataset);

            Single[] value = { Single.MinValue, -1.1f, 0.0f, -1.3f, Single.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Single.MinValue, array.GetValue(0));
            Assert.AreEqual(Single.MaxValue, array.GetValue(4));

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

            Hdf5Dataset dataset = file.Datasets.Add("dataset1", Hdf5DataTypes.Double, properties);
            Assert.IsNotNull(dataset);

            Double[] value = { Double.MinValue, -1.1d, 0.0d, -1.3d, Double.MaxValue };
            dataset.SetData(value);

            var array = dataset.GetData();
            Assert.AreEqual(1, array.Rank);
            Assert.AreEqual(5, array.GetLength(0));
            Assert.AreEqual(Double.MinValue, array.GetValue(0));
            Assert.AreEqual(Double.MaxValue, array.GetValue(4));

            file.Close();
        }

  

    }
}
