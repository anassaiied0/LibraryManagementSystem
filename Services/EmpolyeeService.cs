using System;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public void Add(Employee employee)
        {
            ValidateEmployee(employee);
            _employeeRepository.Add(employee);
        }

        public void Update(Employee employee)
        {
            if (_employeeRepository.GetById(employee.Id) == null)
                throw new NotFoundException(nameof(Employee), employee.Id);

            ValidateEmployee(employee);
            _employeeRepository.Update(employee);
        }

        public List<Employee> GetAll() => _employeeRepository.GetAll();

        public Employee? GetById(int id) => _employeeRepository.GetById(id);

        public void Delete(int id)
        {
            if (_employeeRepository.GetById(id) == null)
                throw new NotFoundException(nameof(Employee), id);

            _employeeRepository.Delete(id);
        }

        public List<Employee> GetByDepartment(int departmentId) => _employeeRepository.GetByDepartment(departmentId);

        public List<Employee> GetActive() => _employeeRepository.GetActive();

        public List<Employee> GetByHireDate(DateTime date) => _employeeRepository.GetByHireDate(date);

        public List<Employee> GetOrderedBySalary() => _employeeRepository.GetOrderedBySalary();

        public List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle)
            => _employeeRepository.Search(id, firstName, lastName, departmentId, jobTitle);

        private void ValidateEmployee(Employee employee)
        {
            Guard.AgainstNullOrWhiteSpace(employee.FirstName, nameof(employee.FirstName));
            Guard.AgainstNullOrWhiteSpace(employee.LastName, nameof(employee.LastName));
            Guard.AgainstInvalidEmail(employee.Email);
            Guard.AgainstNonPositive(employee.Salary, nameof(employee.Salary));
            Guard.AgainstNullOrWhiteSpace(employee.JobTitle, nameof(employee.JobTitle));
            Guard.AgainstFutureDate(employee.HireDate, nameof(employee.HireDate));

            if (_departmentRepository.GetById(employee.DepartmentId) == null)
                throw new NotFoundException(nameof(Department), employee.DepartmentId);

            var existingEmployee = _employeeRepository.GetByEmail(employee.Email);
            if (existingEmployee != null && existingEmployee.Id != employee.Id)
                throw new ConflictException($"An employee with email '{employee.Email}' already exists.");
        }
    }
}