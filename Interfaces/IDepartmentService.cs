using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IDepartmentService
    {
        void Add(Department department);
        List<Department> GetAll();
        Department? GetById(int id);
        void Update(Department department);
        void Delete(int id);
    }
}