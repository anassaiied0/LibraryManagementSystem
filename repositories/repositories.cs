using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class DepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public List<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department? GetById(int id)
        {
            return _context.Departments.FirstOrDefault(d => d.Id == id);
        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var department = GetById(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
        }
    }

    public class EmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.Include(e => e.Department).ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var employee = GetById(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public List<Employee> GetByDepartment(int departmentId)
        {
            return _context.Employees.Where(e => e.DepartmentId == departmentId).ToList();
        }

        public List<Employee> GetActive()
        {
            return _context.Employees.Where(e => e.IsActive).ToList();
        }

        public List<Employee> GetByHireDate(DateTime date)
        {
            return _context.Employees.Where(e => e.HireDate.Date == date.Date).ToList();
        }

        public List<Employee> GetOrderedBySalary()
        {
            return _context.Employees.OrderByDescending(e => e.Salary).ToList();
        }

        public List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle)
        {
            var query = _context.Employees.AsQueryable();

            if (id.HasValue) query = query.Where(e => e.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(firstName)) query = query.Where(e => e.FirstName.Contains(firstName));
            if (!string.IsNullOrWhiteSpace(lastName)) query = query.Where(e => e.LastName.Contains(lastName));
            if (departmentId.HasValue) query = query.Where(e => e.DepartmentId == departmentId.Value);
            if (!string.IsNullOrWhiteSpace(jobTitle)) query = query.Where(e => e.JobTitle.Contains(jobTitle));

            return query.ToList();
        }
    }

    public class MemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public List<Member> GetAll()
        {
            return _context.Members.ToList();
        }

        public Member? GetById(int id)
        {
            return _context.Members.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var member = GetById(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
            _context.SaveChanges();
        }
    }

    public class BookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.FirstOrDefault(x => x.Id == id);
        }

        public Book? GetByTitle(string title)
        {
            return _context.Books.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }

    public class BorrowingRepository
    {
        private readonly AppDbContext _context;

        public BorrowingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            _context.SaveChanges();
        }

        public List<BorrowRecord> GetAll()
        {
            return _context.BorrowRecords.Include(b => b.Member).Include(b => b.Book).ToList();
        }

        public BorrowRecord? GetActiveByBookId(int bookId)
        {
            return _context.BorrowRecords.FirstOrDefault(r => r.BookId == bookId && r.ReturnDate == null);
        }

        public void Update(BorrowRecord record)
        {
            _context.BorrowRecords.Update(record);
            _context.SaveChanges();
        }
    }
}