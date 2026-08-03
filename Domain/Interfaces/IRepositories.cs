using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Domain.Interfaces
{
    using LibraryManagementSystem.Domain.Entities;

    public interface IMemberRepository
    {
        void Add(Member member);
        List<Member> GetAll();
        Member? GetById(int id);
        Member? GetByEmail(string email);
        void Delete(int id);
        void Update(Member member);
    }

    public interface IBookRepository
    {
        void Add(Book book);
        List<Book> GetAll();
        Book? GetById(int id);
        Book? GetByTitle(string title);
        Book? GetByISBN(string isbn);
        void Update(Book book);
        void Delete(int id);
    }

    public interface IBorrowingRepository
    {
        void Add(BorrowRecord record);
        List<BorrowRecord> GetAll();
        BorrowRecord? GetActiveByBookId(int bookId);
        void Update(BorrowRecord record);
    }

    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        List<Employee> GetAll();
        Employee? GetById(int id);
        Employee? GetByEmail(string email);
        void Update(Employee employee);
        void Delete(int id);
        List<Employee> GetByDepartment(int departmentId);
        List<Employee> GetActive();
        List<Employee> GetByHireDate(DateTime date);
        List<Employee> GetOrderedBySalary();
        List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle);
    }

    public interface IDepartmentRepository
    {
        void Add(Department department);
        List<Department> GetAll();
        Department? GetById(int id);
        Department? GetByName(string name);
        void Update(Department department);
        void Delete(int id);
    }
}
