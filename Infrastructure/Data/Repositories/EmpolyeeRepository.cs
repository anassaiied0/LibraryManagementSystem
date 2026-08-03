using System;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to save the employee to the database.", ex);
            }
        }

        public List<Employee> GetAll()
        {
            try
            {
                return _context.Employees.Include(e => e.Department).AsNoTracking().ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve employees from the database.", ex);
            }
        }

        public Employee? GetById(int id)
        {
            try
            {
                return _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve employee with id {id}.", ex);
            }
        }

        public Employee? GetByEmail(string email)
        {
            try
            {
                return _context.Employees.FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve employee with email '{email}'.", ex);
            }
        }

        public void Update(Employee employee)
        {
            try
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to update the employee in the database.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException($"Failed to delete employee with id {id}.", ex);
            }
        }

        public List<Employee> GetByDepartment(int departmentId)
        {
            try
            {
                return _context.Employees.Where(e => e.DepartmentId == departmentId).ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve employees for department {departmentId}.", ex);
            }
        }

        public List<Employee> GetActive()
        {
            try
            {
                return _context.Employees.Where(e => e.IsActive).ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve active employees.", ex);
            }
        }

        public List<Employee> GetByHireDate(DateTime date)
        {
            try
            {
                return _context.Employees.Where(e => e.HireDate.Date == date.Date).ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve employees by hire date.", ex);
            }
        }

        public List<Employee> GetOrderedBySalary()
        {
            try
            {
                return _context.Employees.OrderByDescending(e => e.Salary).ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve employees ordered by salary.", ex);
            }
        }

        public List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle)
        {
            try
            {
                var query = _context.Employees.AsQueryable();

                if (id.HasValue) query = query.Where(e => e.Id == id.Value);
                if (!string.IsNullOrWhiteSpace(firstName)) query = query.Where(e => e.FirstName.Contains(firstName));
                if (!string.IsNullOrWhiteSpace(lastName)) query = query.Where(e => e.LastName.Contains(lastName));
                if (departmentId.HasValue) query = query.Where(e => e.DepartmentId == departmentId.Value);
                if (!string.IsNullOrWhiteSpace(jobTitle)) query = query.Where(e => e.JobTitle.Contains(jobTitle));

                return query.ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to search employees.", ex);
            }
        }
    }
}