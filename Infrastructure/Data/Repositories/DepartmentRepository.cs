using LibraryManagementSystem.Data;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to save the department to the database.", ex);
            }
        }

        public List<Department> GetAll()
        {
            try
            {
                return _context.Departments.AsNoTracking().ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve departments from the database.", ex);
            }
        }

        public Department? GetById(int id)
        {
            try
            {
                return _context.Departments.FirstOrDefault(d => d.Id == id);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve department with id {id}.", ex);
            }
        }

        public Department? GetByName(string name)
        {
            try
            {
                return _context.Departments.FirstOrDefault(d => d.Name.ToLower() == name.ToLower());
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve department with name '{name}'.", ex);
            }
        }

        public void Update(Department department)
        {
            try
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to update the department in the database.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var department = _context.Departments.FirstOrDefault(d => d.Id == id);
                if (department != null)
                {
                    _context.Departments.Remove(department);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                // Most likely FK violation: employees still reference this department (DeleteBehavior.Restrict)
                throw new DataAccessException($"Failed to delete department with id {id}. It may still have employees assigned to it.", ex);
            }
        }
    }
}