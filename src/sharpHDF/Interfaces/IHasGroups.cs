using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Interfaces
{
    public interface IHasGroups
    {
        Hdf5Groups Groups { get; }
    }
}
