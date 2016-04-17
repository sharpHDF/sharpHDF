/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using sharpHDF.Library.Objects;

namespace sharpHDF.Library.Interfaces
{
    public interface IHasGroups
    {
        Hdf5Groups Groups { get; }
    }
}
