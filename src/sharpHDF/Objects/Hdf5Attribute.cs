namespace sharpHDF.Library.Objects
{
    /// <summary>
    /// Contains HDF5 Attribures
    /// </summary>
    public class Hdf5Attribute : AbstractHdf5Object
    {
        /// <summary>
        /// Name of the Attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// String value of the Attribute.
        /// </summary>
        public object Value { get; set; }
    }
}
