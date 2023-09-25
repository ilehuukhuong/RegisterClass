using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class TuitionFeeRepository : ITuitionFeeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TuitionFeeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddTuitionFee(TuitionFee tuitionFee)
        {
            _context.TuitionFees.Add(tuitionFee);
        }

        public bool DeleteTuitionFee(int id)
        {
            _context.TuitionFees.Remove(_context.TuitionFees.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<TuitionFee> GetTuitionFeeById(int id)
        {
            return await _context.TuitionFees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<StudentTuitionFeeDto>> GetTuitionFees(SearchParams searchParams)
        {
            var query = _context.TuitionFees.AsQueryable();

            if (searchParams.Search != null) query = query.Where(u => u.AppUser.Email.Contains(searchParams.Search.ToLower()) || u.AppUser.PhoneNumber.Contains(searchParams.Search) || u.AppUser.UserName.Contains(searchParams.Search.ToLower()));

            return await PagedList<StudentTuitionFeeDto>.CreateAsync(query.AsNoTracking().ProjectTo<StudentTuitionFeeDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void UpdateTuitionFee(TuitionFee tuitionFee)
        {
            _context.TuitionFees.Update(tuitionFee);
        }
    }
}
