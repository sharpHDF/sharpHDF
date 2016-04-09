namespace CSharpHDF5.Objects
{
    /// <summary>
    /// Details on a particular dimension in a dataset.
    /// </summary>
    public class Hdf5DimensionProperty
    {
        public Hdf5DimensionProperty()
        {
            CurrentSize = 1;
            MaximumSize = ulong.MaxValue;
        }

        /// <summary>
        /// Current number of elements in the dataset.
        /// </summary>
        public ulong CurrentSize { get; set; }
        
        /// <summary>
        /// Maximum size of a dataset.  Can only be set at creation of the dataset.
        /// </summary>
        public ulong MaximumSize { get; set; }
    }
}
