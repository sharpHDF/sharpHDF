using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Interfaces
{
    interface IHasAttributes
    {
        Hdf5Attributes Attributes { get; }
    }
}
