using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ITuitionFeeRepository
    {
        void AddTuitionFee(TuitionFee tuitionFee);
        bool DeleteTuitionFee(int id);
        void UpdateTuitionFee(TuitionFee tuitionFee);
        Task<TuitionFee> GetTuitionFeeById(int id);
        Task<PagedList<StudentTuitionFeeDto>> GetTuitionFees(SearchParams searchParams);
    }
}
