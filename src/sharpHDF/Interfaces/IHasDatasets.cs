using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Interfaces
{
    public interface IHasDatasets
    {
        Hdf5Datasets Datasets { get; }
    }
}
