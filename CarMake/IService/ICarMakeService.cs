using CarMake.Core;
using CarMake.Core.Filters;

namespace CarMake.IService
{
    public interface ICarMakeService
    {
        Task<List<string>> GetCars(CarFilter filter);
    }
}
