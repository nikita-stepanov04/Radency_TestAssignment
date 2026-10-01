using Radency_TestAssignment.Application.DTOs;

namespace Radency_TestAssignment.Application.IBusinessServices.Properties
{
    public interface IPropertyBS
    {
        Task<int> AddAsync(AddPropertyDTO property);
    }
}
