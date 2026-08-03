using System;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IEmployeeService
    {
        void Add(Employee employee);
        void Update(Employee employee);
        List<Employee> GetAll();
        Employee? GetById(int id);
        void Delete(int id);
        List<Employee> GetByDepartment(int departmentId);
        List<Employee> GetActive();
        List<Employee> GetByHireDate(DateTime date);
        List<Employee> GetOrderedBySalary();
        List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle);
    }
}