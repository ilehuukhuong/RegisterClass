﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<DepartmentFaculty> DepartmentFaculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<GradeCategory> GradeCategories { get; set; }
        public DbSet<CourseGradeCategory> CourseGradeCategories { get; set; }
        public DbSet<Timetable> Timetables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentClass>()
                .HasKey(k => new { k.UserId, k.ClassId });

            builder.Entity<StudentClass>()
                .HasOne(u => u.User)
                .WithMany(u => u.StudentClasses)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentClass>()
                .HasOne(l => l.Class)
                .WithMany(u => u.StudentClasses)
                .HasForeignKey(l => l.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CourseGradeCategory>()
                .HasKey(k => new { k.GradeCategoryId, k.CourseId });

            builder.Entity<CourseGradeCategory>()
                .HasOne(u => u.Course)
                .WithMany(u => u.CourseGradeCategories)
                .HasForeignKey(u => u.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CourseGradeCategory>()
                .HasOne(l => l.GradeCategory)
                .WithMany(u => u.CourseGradeCategories)
                .HasForeignKey(l => l.GradeCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}
