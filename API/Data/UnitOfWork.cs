﻿using AutoMapper;
using API.Data.Repository;
using API.Interfaces;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IGenderRepository GenderRepository => new GenderRepository(_context);
        public ICourseRepository CourseRepository => new CourseRepository(_context, _mapper);
        public IClassRepository ClassRepository => new ClassRepository(_context, _mapper);
        public IDepartmentFacultyRepository DepartmentFacultyRepository => new DepartmentFacultyRepository(_context, _mapper);
        public ISemesterRepository SemesterRepository => new SemesterRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
