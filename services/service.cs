using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepository _repository;

        public DepartmentService(DepartmentRepository repository)
        {
            _repository = repository;
        }

        public void Add(Department department)
        {
            _repository.Add(department);
        }

        public List<Department> GetAll()
        {
            return _repository.GetAll();
        }

        public Department? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Update(Department department)
        {
            _repository.Update(department);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }

    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly DepartmentRepository _departmentRepository;

        public EmployeeService(EmployeeRepository employeeRepository, DepartmentRepository departmentRepository)
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
            ValidateEmployee(employee);
            _employeeRepository.Update(employee);
        }

        public List<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public Employee? GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        public void Delete(int id)
        {
            _employeeRepository.Delete(id);
        }

        public List<Employee> GetByDepartment(int departmentId)
        {
            return _employeeRepository.GetByDepartment(departmentId);
        }

        public List<Employee> GetActive()
        {
            return _employeeRepository.GetActive();
        }

        public List<Employee> GetByHireDate(DateTime date)
        {
            return _employeeRepository.GetByHireDate(date);
        }

        public List<Employee> GetOrderedBySalary()
        {
            return _employeeRepository.GetOrderedBySalary();
        }

        public List<Employee> Search(int? id, string? firstName, string? lastName, int? departmentId, string? jobTitle)
        {
            return _employeeRepository.Search(id, firstName, lastName, departmentId, jobTitle);
        }

        private void ValidateEmployee(Employee employee)
        {
            if (employee.Salary <= 0)
            {
                throw new InvalidOperationException("Salary must be greater than zero.");
            }

            if (_departmentRepository.GetById(employee.DepartmentId) == null)
            {
                throw new InvalidOperationException("Employee must belong to an existing department.");
            }

            var existingEmployee = _employeeRepository.GetAll()
                .FirstOrDefault(e => e.Email == employee.Email && e.Id != employee.Id);

            if (existingEmployee != null)
            {
                throw new InvalidOperationException("Email must be unique.");
            }
        }
    }

    public class MemberService
    {
        private readonly MemberRepository _repository;

        public MemberService(MemberRepository repository)
        {
            _repository = repository;
        }

        public void Register(Member member) => _repository.Add(member);
        public List<Member> GetMembers() => _repository.GetAll();
        public Member? Search(int id) => _repository.GetById(id);
        public void Delete(int id) => _repository.Delete(id);
        public void Update(Member member) => _repository.Update(member);
    }

    public class BookService
    {
        private readonly BookRepository _repository;

        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public void Add(Book book) => _repository.Add(book);
        public List<Book> GetBooks() => _repository.GetAll();
        public Book? Search(int id) => _repository.GetById(id);
        public Book? Search(string title) => _repository.GetByTitle(title);
        public void Delete(int id) => _repository.Delete(id);
        public void Update(Book book) => _repository.Update(book);
    }

    public class BorrowingService
    {
        private readonly BorrowingRepository _repository;
        private readonly MemberService _memberService;
        private readonly BookService _bookService;

        public BorrowingService(BorrowingRepository repository, MemberService memberService, BookService bookService)
        {
            _repository = repository;
            _memberService = memberService;
            _bookService = bookService;
        }

        public BorrowRecord BorrowBook(int memberId, int bookId)
        {
            var member = _memberService.Search(memberId) ?? throw new InvalidOperationException("Member not found.");
            var book = _bookService.Search(bookId) ?? throw new InvalidOperationException("Book not found.");

            if (!book.IsAvailable) throw new InvalidOperationException("This book is already borrowed.");

            var borrowRecord = new BorrowRecord
            {
                MemberId = member.Id,
                BookId = book.Id,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            _repository.Add(borrowRecord);

            book.IsAvailable = false;
            _bookService.Update(book);

            return borrowRecord;
        }

        public BorrowRecord ReturnBook(int bookId)
        {
            var book = _bookService.Search(bookId) ?? throw new InvalidOperationException("Book not found.");
            var borrowRecord = _repository.GetActiveByBookId(bookId) ?? throw new InvalidOperationException("This book is not currently borrowed.");

            borrowRecord.ReturnDate = DateTime.Now;
            _repository.Update(borrowRecord);

            book.IsAvailable = true;
            _bookService.Update(book);

            return borrowRecord;
        }

        public List<BorrowRecord> GetBorrowRecords()
        {
            return _repository.GetAll();
        }
    }
}