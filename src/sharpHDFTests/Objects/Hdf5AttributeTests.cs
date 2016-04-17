/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using sharpHDF.Library.Enums;
using sharpHDF.Library.Exceptions;
using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Tests.Objects
{
    [TestFixture]
    public class Hdf5AttributeTests : BaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DirectoryName = @"c:\temp\hdf5tests\attributetests";

            CleanDirectory();
        }

        [Test]
        public void CreateAttributeOnFile()
        {
            string fileName = GetFilename("createattributeonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            file.Attributes.Add("attribute1", "test");
            file.Attributes.Add("attribute2", 5);
            Assert.AreEqual(2, file.Attributes.Count);

            file.Close();
        }

        [Test]
        public void CreateAttributeTwiceOnFile()
        {
            string fileName = GetFilename("createattributetwiceonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            file.Attributes.Add("attribute1", "test");

            try
            {
                file.Attributes.Add("attribute1", "test2");
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5AttributeAlreadyExistException>(ex);
            }
        }

        [Test]
        public void CreateAttributeTwiceOnGroup()
        {
            string fileName = GetFilename("createattributetwiceongroup.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("test");
            group.Attributes.Add("attribute1", "test");

            try
            {
                group.Attributes.Add("attribute1", "test2");
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5AttributeAlreadyExistException>(ex);
            }
        }

        [Test]
        public void CreateAttributeTwiceOnDataset()
        {
            string fileName = GetFilename("createattributetwiceondataset.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("test");
            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);

            dataset.Attributes.Add("attribute1", "test");

            try
            {
                dataset.Attributes.Add("attribute1", "test2");
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5AttributeAlreadyExistException>(ex);
            }
        }

        [Test]
        public void UpdateAttributeWithMismatchOnFile()
        {
            string fileName = GetFilename("updateattributewithmistmachonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Attribute attribute = file.Attributes.Add("attribute1", "test");

            try
            {
                attribute.Value = 5;

                file.Attributes.Update(attribute);

                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5TypeMismatchException>(ex);
            }
        }

        [Test]
        public void UpdateAttributeWithMismatchOnGroup()
        {
            string fileName = GetFilename("updateattributewithmistmachongroup.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group");
            Hdf5Attribute attribute = group.Attributes.Add("attribute1", "test");

            try
            {
                attribute.Value = 5;

                group.Attributes.Update(attribute);

                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5TypeMismatchException>(ex);
            }
        }

        [Test]
        public void UpdateAttributeWithMismatchOnDataset()
        {
            string fileName = GetFilename("updateattributewithmistmachondataset.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);

            Hdf5Attribute attribute = dataset.Attributes.Add("attribute1", "test");

            try
            {
                attribute.Value = 5;

                dataset.Attributes.Update(attribute);

                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception ex)
            {
                file.Close();
                Assert.IsInstanceOf<Hdf5TypeMismatchException>(ex);
            }
        }

        [Test]
        public void UpdateStringAttributeOnFile()
        {
            string fileName = GetFilename("updatestringattributeonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Attribute attribute = file.Attributes.Add("attribute1", "test");

            attribute.Value = "test2";
            file.Attributes.Update(attribute);
            file.Close();

            file = new Hdf5File(fileName);
            attribute = file.Attributes[0];
            Assert.AreEqual("test2", attribute.Value);
        }

        [Test]
        public void UpdateStringAttributeOnGroup()
        {
            string fileName = GetFilename("updatestringattributeongroup.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group");
            Hdf5Attribute attribute = group.Attributes.Add("attribute1", "test");
            attribute.Value = "test2";
            group.Attributes.Update(attribute);
            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            attribute = group.Attributes[0];
            Assert.AreEqual("test2", attribute.Value);
        }

        [Test]
        public void UpdateStringAttributeOnDataset()
        {
            string fileName = GetFilename("updatestringattributeondataset.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);

            Hdf5Attribute attribute = dataset.Attributes.Add("attribute1", "test");
            attribute.Value = "test2";
            dataset.Attributes.Update(attribute);
            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            dataset = group.Datasets[0];
            attribute = dataset.Attributes[0];
            Assert.AreEqual("test2", attribute.Value);
        }

        [Test]
        public void GetAttributeOnFile()
        {
            string fileName = GetFilename("getattributeonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            file.Attributes.Add("attribute1", "test");
            file.Attributes.Add("attribute2", 5);
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

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");
            group.Attributes.Add("attribute1", "test");
            group.Attributes.Add("attribute2", 5);
            Assert.AreEqual(2, group.Attributes.Count);

            file.Close();
        }

        [Test]
        public void GetAttributeOnGroup()
        {
            string fileName = GetFilename("getattributeongroup.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");
            group.Attributes.Add("attribute1", "test");
            group.Attributes.Add("attribute2", 5);
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

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty {CurrentSize = 1};
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);
            dataset.Attributes.Add("attribute1", "test");
            dataset.Attributes.Add("attribute2", 5);
            Assert.AreEqual(2, dataset.Attributes.Count);

            file.Close();
        }

        [Test]
        public void GetAttributeOnDataset()
        {
            string fileName = GetFilename("getattributeondataset.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);
            dataset.Attributes.Add("attribute1", "test");
            dataset.Attributes.Add("attribute2", 5);
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

        [Test]
        public void DeleteAttributeOnFile()
        {
            string fileName = GetFilename("deleteattributeonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            file.Attributes.Add("attribute1", "test");
            file.Attributes.Add("attribute2", 5);
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

            file.Attributes.Delete(attribute1);
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);

            file.Close();

            file = new Hdf5File(fileName);
            attibutes = file.Attributes;
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }

        [Test]
        public void DeleteAttributeOnGroup()
        {
            string fileName = GetFilename("deleteattributeongroup.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");
            group.Attributes.Add("attribute1", "test");
            group.Attributes.Add("attribute2", 5);
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

            group.Attributes.Delete(attribute1);
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);

            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            attibutes = group.Attributes;
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }

        [Test]
        public void DeleteAttributeOnDataset()
        {
            string fileName = GetFilename("deleteattributeondataset.h5");

            Hdf5File file = Hdf5File.Create(fileName);
            Hdf5Group group = file.Groups.Add("group1");

            List<Hdf5DimensionProperty> dimensionProps = new List<Hdf5DimensionProperty>();
            Hdf5DimensionProperty prop = new Hdf5DimensionProperty { CurrentSize = 1 };
            dimensionProps.Add(prop);

            Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int32, dimensionProps);
            dataset.Attributes.Add("attribute1", "test");
            dataset.Attributes.Add("attribute2", 5);
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

            dataset.Attributes.Delete(attribute1);
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);

            file.Close();

            file = new Hdf5File(fileName);
            group = file.Groups[0];
            dataset = group.Datasets[0];
            attibutes = dataset.Attributes;
            Assert.AreEqual(1, attibutes.Count);
            attribute2 = attibutes[0];
            Assert.AreEqual("attribute2", attribute2.Name);
            Assert.AreEqual(5, attribute2.Value);
        }

        [Test]
        public void AllAttributeTypesOnFile()
        {
            string fileName = GetFilename("allattributetypesonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);

            file.Attributes.Add("attributea", "test");

            sbyte b = sbyte.MaxValue;
            file.Attributes.Add("attributeb", b);

            Int16 c = Int16.MaxValue;
            file.Attributes.Add("attributec", c);

            Int32 d = Int32.MaxValue;
            file.Attributes.Add("attributed", d);

            Int64 e = Int64.MaxValue;
            file.Attributes.Add("attributee", e);

            byte f = Byte.MaxValue;
            file.Attributes.Add("attibutef", f);

            UInt16 g = UInt16.MaxValue;
            file.Attributes.Add("attributeg", g);

            UInt32 h = UInt32.MaxValue;
            file.Attributes.Add("attibuteh", h);

            UInt64 i = UInt64.MaxValue;
            file.Attributes.Add("attributei", i);

            float j = float.MaxValue;
            file.Attributes.Add("attibutej", j);

            double k = double.MaxValue;
            file.Attributes.Add("attributek", k);

            Assert.AreEqual(11, file.Attributes.Count);

            file.Close();

            file = new Hdf5File(fileName);
            var attibutes = file.Attributes;
            Assert.AreEqual(11, attibutes.Count);

            var attribute1 = attibutes[0];
            Assert.AreEqual("test", attribute1.Value);

            var attribute2 = attibutes[1];
            Assert.AreEqual(sbyte.MaxValue, attribute2.Value);

            var attribute3 = attibutes[2];
            Assert.AreEqual(Int16.MaxValue, attribute3.Value);

            var attribute4 = attibutes[3];
            Assert.AreEqual(Int32.MaxValue, attribute4.Value);

            var attribute5 = attibutes[4];
            Assert.AreEqual(Int64.MaxValue, attribute5.Value);

            var attribute6 = attibutes[5];
            Assert.AreEqual(byte.MaxValue, attribute6.Value);

            var attribute7 = attibutes[6];
            Assert.AreEqual(UInt16.MaxValue, attribute7.Value);

            var attribute8 = attibutes[7];
            Assert.AreEqual(UInt32.MaxValue, attribute8.Value);

            var attribute9 = attibutes[8];
            Assert.AreEqual(UInt64.MaxValue, attribute9.Value);

            var attribute10 = attibutes[9];
            Assert.AreEqual(float.MaxValue, attribute10.Value);

            var attribute11 = attibutes[10];
            Assert.AreEqual(double.MaxValue, attribute11.Value);
        }

        [Test]
        public void UpdateAllAttributeTypesOnFile()
        {
            string fileName = GetFilename("updateallattributetypesonfile.h5");

            Hdf5File file = Hdf5File.Create(fileName);

            var atta = file.Attributes.Add("attributea", "test");
            atta.Value = "test2";
            file.Attributes.Update(atta);

            sbyte b = sbyte.MaxValue;
            var attb = file.Attributes.Add("attributeb", b);
            attb.Value = sbyte.MinValue;
            file.Attributes.Update(attb);

            Int16 c = Int16.MaxValue;
            var attc = file.Attributes.Add("attributec", c);
            attc.Value = Int16.MinValue;
            file.Attributes.Update(attc);

            Int32 d = Int32.MaxValue;
            var attd = file.Attributes.Add("attributed", d);
            attd.Value = Int32.MinValue;
            file.Attributes.Update(attd);

            Int64 e = Int64.MaxValue;
            var atte = file.Attributes.Add("attributee", e);
            atte.Value = Int64.MinValue;
            file.Attributes.Update(atte);

            byte f = Byte.MaxValue;
            var attf = file.Attributes.Add("attibutef", f);
            attf.Value = Byte.MinValue;
            file.Attributes.Update(attf);

            UInt16 g = UInt16.MaxValue;
            var attg = file.Attributes.Add("attributeg", g);
            attg.Value = UInt16.MinValue;
            file.Attributes.Update(attg);

            UInt32 h = UInt32.MaxValue;
            var atth = file.Attributes.Add("attibuteh", h);
            atth.Value = UInt32.MinValue;
            file.Attributes.Update(atth);

            UInt64 i = UInt64.MaxValue;
            var atti = file.Attributes.Add("attributei", i);
            atti.Value = UInt64.MinValue;
            file.Attributes.Update(atti);

            float j = float.MaxValue;
            var attj = file.Attributes.Add("attibutej", j);
            attj.Value = float.MinValue;
            file.Attributes.Update(attj);

            double k = double.MaxValue;
            var attk = file.Attributes.Add("attributek", k);
            attk.Value = double.MinValue;
            file.Attributes.Update(attk);

            Assert.AreEqual(11, file.Attributes.Count);

            file.Close();

            file = new Hdf5File(fileName);
            var attibutes = file.Attributes;
            Assert.AreEqual(11, attibutes.Count);

            var attribute1 = attibutes[0];
            Assert.AreEqual("test2", attribute1.Value);

            var attribute2 = attibutes[1];
            Assert.AreEqual(sbyte.MinValue, attribute2.Value);

            var attribute3 = attibutes[2];
            Assert.AreEqual(Int16.MinValue, attribute3.Value);

            var attribute4 = attibutes[3];
            Assert.AreEqual(Int32.MinValue, attribute4.Value);

            var attribute5 = attibutes[4];
            Assert.AreEqual(Int64.MinValue, attribute5.Value);

            var attribute6 = attibutes[5];
            Assert.AreEqual(byte.MinValue, attribute6.Value);

            var attribute7 = attibutes[6];
            Assert.AreEqual(UInt16.MinValue, attribute7.Value);

            var attribute8 = attibutes[7];
            Assert.AreEqual(UInt32.MinValue, attribute8.Value);

            var attribute9 = attibutes[8];
            Assert.AreEqual(UInt64.MinValue, attribute9.Value);

            var attribute10 = attibutes[9];
            Assert.AreEqual(float.MinValue, attribute10.Value);

            var attribute11 = attibutes[10];
            Assert.AreEqual(double.MinValue, attribute11.Value);
        }
    }
}
