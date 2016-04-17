/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using NUnit.Framework;
using sharpHDF.Library.Exceptions;
using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Tests.Objects
{
    [TestFixture]
    public class Hdf5TypeTests :BaseTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DirectoryName = @"c:\temp\hdf5tests\typetests";

            CleanDirectory();
        }

        [Test]
        public void AttemptMoney()
        {
            string filename = GetFilename("attemptmoney.hdf5");

            var file = Hdf5File.Create(filename);

            try
            {
                decimal money = 123.33m;

                file.Attributes.Add("moneyvalue", money);

                Assert.Fail("Exception was expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<Hdf5UnsupportedDataTypeException>(ex);
            }

            file.Close();
        }
    }
}
