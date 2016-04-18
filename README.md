#sharpHDF

[![codecov.io](https://codecov.io/github/sharpHDF/sharpHDF/coverage.svg?branch=master)](https://codecov.io/github/sharpSDF/sharpHDF5?branch=master)

[![appveyor.com](https://ci.appveyor.com/api/projects/status/github/sharpHDF/sharpHDF)](https://ci.appveyor.com/api/projects/status/github/sharpHDF/sharpHDF)


This library provides a .Net based object oriented approach to accessing HDF5 files.  The library uses the [HDF.PInvoke] (https://github.com/HDFGroup/HDF.PInvoke) library that has been provided by The HDF Group to access the files.  [The HDF Group](https://www.hdfgroup.org/) controls the the specification and licensing to the underlying technology.  HDF stands for Hierarchical Data Format.  For more information on the underlying technology please refer to their site.

Currently the **sharpHDF** library supports a subset of version 1.8 of the HDF5 Specification.

The inital purpose of the library is to assist with an Amateur Radio Astronomy project I am working on.  The sharpHDF library may be of use to others as is or as an example of how to work with the HDF.PInvoke library.

##Supported Features
1. Create HDF5 Files
1. Open HDF5 Files
1. Create Groups on Files or Groups
1. Create Datasets on Files or Groups
1. Add, update and delete Attributes to Files, Groups or Datasets
1. Add and retrieve data to a dataset.


##Installation
1. Install the HDF.PInvoke library from nuget
1. Install the sharpHDF library from nuget

##Sample Usage

###Create File
Creates a file in specified directory.

    Hdf5File file = Hdf5File.Create(@"c:\temp\myfile.h5");

	... Use file as needed ...    

    file.Close();

###Open File
Opens the existing HDF5 file in specified location

    Hdf5File file = new Hdf5File(@"c:\temp\myfile.h5");

  	... Use file as needed ...   
  
    file.Close();
    
###Add Group to File
Adds a group to a newly created file object.

    Hdf5File file = Hdf5File.Create(@"c:\temp\myfile.h5");    
    Hdf5Group group = file.Groups.Add("group1");    

	... Use group as needed ...    

	file.Close();
	
###Add Group to Group
Adds a parent and child group to a newly created file.

    Hdf5File file = Hdf5File.Create(@"c:\temp\myfile.h5");    
    Hdf5Group parent = file.Groups.Add("parentGroup");    
	Hdf5Group childGroup = group.Groups.Add("childGroup");
	
	... Use groups as needed ...    

	file.Close();
	
###Add Dataset to Group
Creates a file and group then adds a dataset to the group.  The dataset is defined as single dimension 5 element array of Signed 8 bit integers.

    Hdf5File file = Hdf5File.Create(filename);

    Hdf5Group group = file.Groups.Add("group1");

    List<Hdf5DimensionProperty> properties = new List<Hdf5DimensionProperty>();
    Hdf5DimensionProperty property = new Hdf5DimensionProperty { CurrentSize = 5 };
    properties.Add(property);

    Hdf5Dataset dataset = group.Datasets.Add("dataset1", Hdf5DataTypes.Int8, properties);
    Int8[] value = {1, 2, 3, 4, 5};
    dataset.SetData(value);

    file.Close();
    
###Reads data from existing dataset
Reads data back from an existing file with a child group.  The child group has a child dataset.

    Hdf5File file = new Hdf5File(@"c:\temp\myfile.h5");
    Hdf5Group group = file.Groups[0];
    Hdf5Dataset dataset = group.Datasets[0];
    Array data = dataset.GetData();
