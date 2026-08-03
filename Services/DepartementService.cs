using System;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public void Add(Department department)
        {
            Validate(department);
            _repository.Add(department);
        }

        public List<Department> GetAll() => _repository.GetAll();

        public Department? GetById(int id) => _repository.GetById(id);

        public void Update(Department department)
        {
            if (_repository.GetById(department.Id) == null)
                throw new NotFoundException(nameof(Department), department.Id);

            Validate(department);
            _repository.Update(department);
        }

        public void Delete(int id)
        {
            if (_repository.GetById(id) == null)
                throw new NotFoundException(nameof(Department), id);

            _repository.Delete(id);
        }

        private void Validate(Department department)
        {
            Guard.AgainstNullOrWhiteSpace(department.Name, nameof(department.Name));

            var existing = _repository.GetByName(department.Name);
            if (existing != null && existing.Id != department.Id)
                throw new ConflictException($"A department named '{department.Name}' already exists.");
        }
    }
}