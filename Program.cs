using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;

var context = new AppDbContext();

var departmentRepo = new DepartmentRepository(context);
var employeeRepo = new EmployeeRepository(context);
var memberRepo = new MemberRepository(context);
var bookRepo = new BookRepository(context);
var borrowingRepo = new BorrowingRepository(context);

var departmentService = new DepartmentService(departmentRepo);
var employeeService = new EmployeeService(employeeRepo, departmentRepo);
var memberService = new MemberService(memberRepo);
var bookService = new BookService(bookRepo);
var borrowingService = new BorrowingService(borrowingRepo, memberService, bookService);

bool exit = false;

while (!exit)
{
    Console.Clear();
    Console.WriteLine("============================");
    Console.WriteLine("Employee Management System");
    Console.WriteLine("============================");
    Console.WriteLine("1. Add Department");
    Console.WriteLine("2. List Departments");
    Console.WriteLine("3. Update Department");
    Console.WriteLine("4. Delete Department");
    Console.WriteLine("5. Add Employee");
    Console.WriteLine("6. List Employees");
    Console.WriteLine("7. Search Employee");
    Console.WriteLine("8. Update Employee");
    Console.WriteLine("9. Delete Employee");
    Console.WriteLine("10. Employees By Department");
    Console.WriteLine("11. Active Employees");
    Console.WriteLine("12. Employees By Hire Date");
    Console.WriteLine("13. Employees Ordered By Salary");
    Console.WriteLine("14. Exit");
    Console.WriteLine("============================");

    int choice = ReadInt("Choose an option: ");
    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case 1: AddDepartment(); break;
            case 2: ListDepartments(); break;
            case 3: UpdateDepartment(); break;
            case 4: DeleteDepartment(); break;
            case 5: AddEmployee(); break;
            case 6: ListEmployees(); break;
            case 7: SearchEmployee(); break;
            case 8: UpdateEmployee(); break;
            case 9: DeleteEmployee(); break;
            case 10: EmployeesByDepartment(); break;
            case 11: ActiveEmployees(); break;
            case 12: EmployeesByHireDate(); break;
            case 13: EmployeesOrderedBySalary(); break;
            case 14: exit = true; break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    if (!exit)
    {
        Console.WriteLine("\nPress Enter to return to the menu...");
        Console.ReadLine();
    }
}

void AddDepartment()
{
    string name = ReadRequiredString("Department Name: ");
    string description = ReadString("Description (optional): ");

    departmentService.Add(new Department { Name = name, Description = description });
    Console.WriteLine("Department added successfully.");
}

void ListDepartments()
{
    var departments = departmentService.GetAll();
    if (!departments.Any()) { Console.WriteLine("No departments found."); return; }

    foreach (var dept in departments)
    {
        Console.WriteLine($"ID: {dept.Id} | Name: {dept.Name} | Description: {dept.Description}");
    }
}

void UpdateDepartment()
{
    int id = ReadInt("Department ID: ");
    var dept = departmentService.GetById(id) ?? throw new InvalidOperationException("Department not found.");

    dept.Name = ReadRequiredString($"New Name ({dept.Name}): ");
    dept.Description = ReadString($"New Description ({dept.Description}): ");

    departmentService.Update(dept);
    Console.WriteLine("Department updated successfully.");
}

void DeleteDepartment()
{
    int id = ReadInt("Department ID to delete: ");
    departmentService.Delete(id);
    Console.WriteLine("Department deleted successfully.");
}

void AddEmployee()
{
    var employee = new Employee
    {
        FirstName = ReadRequiredString("First Name: "),
        LastName = ReadRequiredString("Last Name: "),
        Email = ReadRequiredString("Email: "),
        Salary = ReadDecimal("Salary: "),
        JobTitle = ReadRequiredString("Job Title: "),
        HireDate = ReadDate("Hire Date (yyyy-mm-dd): "),
        DepartmentId = ReadInt("Department ID: ")
    };

    employeeService.Add(employee);
    Console.WriteLine("Employee added successfully.");
}

void ListEmployees()
{
    var employees = employeeService.GetAll();
    if (!employees.Any()) { Console.WriteLine("No employees found."); return; }

    foreach (var emp in employees) DisplayEmployee(emp);
}

void SearchEmployee()
{
    Console.WriteLine("Search by (leave blank to skip):");
    int? id = ReadOptionalInt("Employee ID: ");
    string? firstName = ReadOptionalString("First Name: ");
    string? lastName = ReadOptionalString("Last Name: ");
    int? deptId = ReadOptionalInt("Department ID: ");
    string? jobTitle = ReadOptionalString("Job Title: ");

    var results = employeeService.Search(id, firstName, lastName, deptId, jobTitle);

    if (!results.Any()) { Console.WriteLine("No employees found matching criteria."); return; }
    foreach (var emp in results) DisplayEmployee(emp);
}

void UpdateEmployee()
{
    int id = ReadInt("Employee ID: ");
    var emp = employeeService.GetById(id) ?? throw new InvalidOperationException("Employee not found.");

    emp.FirstName = ReadRequiredString($"First Name ({emp.FirstName}): ");
    emp.LastName = ReadRequiredString($"Last Name ({emp.LastName}): ");
    emp.Email = ReadRequiredString($"Email ({emp.Email}): ");
    emp.Salary = ReadDecimal($"Salary ({emp.Salary}): ");
    emp.JobTitle = ReadRequiredString($"Job Title ({emp.JobTitle}): ");
    emp.DepartmentId = ReadInt($"Department ID ({emp.DepartmentId}): ");

    employeeService.Update(emp);
    Console.WriteLine("Employee updated successfully.");
}

void DeleteEmployee()
{
    int id = ReadInt("Employee ID to delete: ");
    employeeService.Delete(id);
    Console.WriteLine("Employee deleted successfully.");
}

void EmployeesByDepartment()
{
    int deptId = ReadInt("Department ID: ");
    var employees = employeeService.GetByDepartment(deptId);
    if (!employees.Any()) { Console.WriteLine("No employees in this department."); return; }
    foreach (var emp in employees) DisplayEmployee(emp);
}

void ActiveEmployees()
{
    var employees = employeeService.GetActive();
    if (!employees.Any()) { Console.WriteLine("No active employees found."); return; }
    foreach (var emp in employees) DisplayEmployee(emp);
}

void EmployeesByHireDate()
{
    DateTime date = ReadDate("Hire Date (yyyy-mm-dd): ");
    var employees = employeeService.GetByHireDate(date);
    if (!employees.Any()) { Console.WriteLine("No employees hired on this date."); return; }
    foreach (var emp in employees) DisplayEmployee(emp);
}

void EmployeesOrderedBySalary()
{
    var employees = employeeService.GetOrderedBySalary();
    if (!employees.Any()) { Console.WriteLine("No employees found."); return; }
    foreach (var emp in employees) DisplayEmployee(emp);
}

void DisplayEmployee(Employee emp)
{
    Console.WriteLine($"-----------------------------------");
    Console.WriteLine($"ID: {emp.Id} | Name: {emp.FirstName} {emp.LastName}");
    Console.WriteLine($"Email: {emp.Email} | Job: {emp.JobTitle}");
    Console.WriteLine($"Salary: {emp.Salary:C} | Hired: {emp.HireDate:d}");
    Console.WriteLine($"Department: {emp.Department?.Name} | Active: {emp.IsActive}");
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value)) return value;
        Console.WriteLine("Invalid number. Please try again.");
    }
}

int? ReadOptionalInt(string prompt)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();
    return int.TryParse(input, out int value) ? value : null;
}

decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (decimal.TryParse(Console.ReadLine(), out decimal value)) return value;
        Console.WriteLine("Invalid number. Please try again.");
    }
}

DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (DateTime.TryParse(Console.ReadLine(), out DateTime value)) return value;
        Console.WriteLine("Invalid date. Please try again (YYYY-MM-DD).");
    }
}

string ReadString(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? string.Empty;
}

string? ReadOptionalString(string prompt)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();
    return string.IsNullOrWhiteSpace(input) ? null : input;
}

string ReadRequiredString(string prompt)
{
    while (true)
    {
        string value = ReadString(prompt);
        if (!string.IsNullOrWhiteSpace(value)) return value;
        Console.WriteLine("This field cannot be empty.");
    }
}