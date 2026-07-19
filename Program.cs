using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

MemberService memberService = new MemberService();
BookService bookService = new BookService();

bool exit = false;


int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.WriteLine("Invalid number. Please try again.");
    }
}

string ReadString(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? string.Empty;
}

string ReadRequiredString(string prompt)
{
    while (true)
    {
        string value = ReadString(prompt);

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        Console.WriteLine("This field cannot be empty.");
    }
}

int ReadPublicationYear(string prompt)
{
    while (true)
    {
        int year = ReadInt(prompt);

        if (year > 0 && year <= DateTime.Now.Year)
        {
            return year;
        }

        Console.WriteLine(
            $"Publication year must be between 1 and {DateTime.Now.Year}.");
    }
}

void DisplayBook(Book book)
{
    Console.WriteLine("-------------------------");
    Console.WriteLine($"Id: {book.Id}");
    Console.WriteLine($"Title: {book.Title}");
    Console.WriteLine($"Author: {book.Author}");
    Console.WriteLine($"ISBN: {book.ISBN}");
    Console.WriteLine($"Publication Year: {book.PublicationYear}");
    Console.WriteLine($"Category: {book.Category}");
    Console.WriteLine(
        $"Availability: {(book.IsAvailable ? "Available" : "Borrowed")}");
    Console.WriteLine("-------------------------");
}

// ========================================================
// Book Methods
// ========================================================

void AddBook()
{
    Console.WriteLine("Add Book");
    Console.WriteLine("--------");

    int id = ReadInt("Id: ");

    if (bookService.Search(id) != null)
    {
        Console.WriteLine("A book with this Id already exists.");
        return;
    }

    string title = ReadRequiredString("Title: ");
    string author = ReadRequiredString("Author: ");
    string isbn = ReadRequiredString("ISBN: ");
    int publicationYear = ReadPublicationYear("Publication Year: ");
    string category = ReadRequiredString("Category: ");

    Book book = new Book
    {
        Id = id,
        Title = title,
        Author = author,
        ISBN = isbn,
        PublicationYear = publicationYear,
        Category = category,
        IsAvailable = true
    };

    bookService.Add(book);

    Console.WriteLine("Book added successfully.");
}

void UpdateBook()
{
    Console.WriteLine("Update Book");
    Console.WriteLine("-----------");

    int id = ReadInt("Enter Book Id: ");

    Book? existingBook = bookService.Search(id);

    if (existingBook == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    string newTitle =
        ReadString("Enter New Title (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newTitle))
    {
        existingBook.Title = newTitle;
    }

    string newAuthor =
        ReadString("Enter New Author (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newAuthor))
    {
        existingBook.Author = newAuthor;
    }

    string newIsbn =
        ReadString("Enter New ISBN (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newIsbn))
    {
        existingBook.ISBN = newIsbn;
    }

    string newYear =
        ReadString("Enter New Publication Year (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newYear))
    {
        if (!int.TryParse(newYear, out int year) ||
            year <= 0 ||
            year > DateTime.Now.Year)
        {
            Console.WriteLine("Invalid publication year.");
            return;
        }

        existingBook.PublicationYear = year;
    }

    string newCategory =
        ReadString("Enter New Category (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newCategory))
    {
        existingBook.Category = newCategory;
    }

    bookService.Update(existingBook);

    Console.WriteLine("Book updated successfully.");
}

void DeleteBook()
{
    Console.WriteLine("Delete Book");
    Console.WriteLine("-----------");

    int id = ReadInt("Enter Book Id: ");

    Book? existingBook = bookService.Search(id);

    if (existingBook == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    bookService.Delete(id);

    Console.WriteLine("Book deleted successfully.");
}

void ListBooks()
{
    Console.WriteLine("List Books");
    Console.WriteLine("----------");

    var books = bookService.GetBooks();

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (Book book in books)
    {
        DisplayBook(book);
    }
}

void SearchBook()
{
    Console.WriteLine("Search Book");
    Console.WriteLine("-----------");
    Console.WriteLine("1. Search by Id");
    Console.WriteLine("2. Search by Title");

    int searchChoice = ReadInt("Choose search type: ");
    Book? book;

    if (searchChoice == 1)
    {
        int id = ReadInt("Enter Book Id: ");
        book = bookService.Search(id);
    }
    else if (searchChoice == 2)
    {
        string title = ReadRequiredString("Enter Book Title: ");
        book = bookService.Search(title);
    }
    else
    {
        Console.WriteLine("Invalid search option.");
        return;
    }

    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    DisplayBook(book);
}

void RegisterMember()
{
    Console.WriteLine("Register Member");
    Console.WriteLine("---------------");

    int id = ReadInt("Id: ");

    if (memberService.Search(id) != null)
    {
        Console.WriteLine("A member with this Id already exists.");
        return;
    }

    string name = ReadRequiredString("Name: ");
    string email = ReadRequiredString("Email: ");
    string phone = ReadRequiredString("Phone: ");

    Member member = new Member
    {
        Id = id,
        FullName = name,
        Email = email,
        PhoneNumber = phone
    };

    memberService.Register(member);

    Console.WriteLine("Member added successfully.");
}

void UpdateMember()
{
    Console.WriteLine("Update Member");
    Console.WriteLine("-------------");

    int id = ReadInt("Enter Member Id: ");

    Member? existingMember = memberService.Search(id);

    if (existingMember == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    string newName =
        ReadString("Enter New Name (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newName))
    {
        existingMember.FullName = newName;
    }

    string newEmail =
        ReadString("Enter New Email (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newEmail))
    {
        existingMember.Email = newEmail;
    }

    string newPhone =
        ReadString("Enter New Phone (leave blank to keep current): ");

    if (!string.IsNullOrWhiteSpace(newPhone))
    {
        existingMember.PhoneNumber = newPhone;
    }

    memberService.Update(existingMember);

    Console.WriteLine("Member updated successfully.");
}

void DeleteMember()
{
    Console.WriteLine("Delete Member");
    Console.WriteLine("-------------");

    int id = ReadInt("Enter Member Id: ");

    Member? existingMember = memberService.Search(id);

    if (existingMember == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    memberService.Delete(id);

    Console.WriteLine("Member deleted successfully.");
}

void ListMembers()
{
    Console.WriteLine("List Members");
    Console.WriteLine("------------");

    var members = memberService.GetMembers();

    if (members.Count == 0)
    {
        Console.WriteLine("No members found.");
        return;
    }

    foreach (Member member in members)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"Id: {member.Id}");
        Console.WriteLine($"Name: {member.FullName}");
        Console.WriteLine($"Email: {member.Email}");
        Console.WriteLine($"Phone: {member.PhoneNumber}");
        Console.WriteLine("-------------------------");
    }
}


void BorrowBook()
{
    Console.WriteLine("Borrow Book");
    Console.WriteLine("Borrowing service is not connected yet.");
}

void ReturnBook()
{
    Console.WriteLine("Return Book");
    Console.WriteLine("Borrowing service is not connected yet.");
}

// ========================================================
// Console Menu
// ========================================================

void DisplayMenu()
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine("    Library Management System");
    Console.WriteLine("=================================");
    Console.WriteLine("1.  Add Book");
    Console.WriteLine("2.  Update Book");
    Console.WriteLine("3.  Delete Book");
    Console.WriteLine("4.  List Books");
    Console.WriteLine("5.  Search Book");
    Console.WriteLine("6.  Register Member");
    Console.WriteLine("7.  Update Member");
    Console.WriteLine("8.  Delete Member");
    Console.WriteLine("9.  List Members");
    Console.WriteLine("10. Borrow Book");
    Console.WriteLine("11. Return Book");
    Console.WriteLine("12. Exit");
    Console.WriteLine("=================================");
}

// ========================================================
// Main Program Loop
// ========================================================

while (!exit)
{
    DisplayMenu();

    int choice = ReadInt("Choose an option: ");

    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case 1:
                AddBook();
                break;

            case 2:
                UpdateBook();
                break;

            case 3:
                DeleteBook();
                break;

            case 4:
                ListBooks();
                break;

            case 5:
                SearchBook();
                break;

            case 6:
                RegisterMember();
                break;

            case 7:
                UpdateMember();
                break;

            case 8:
                DeleteMember();
                break;

            case 9:
                ListMembers();
                break;

            case 10:
                BorrowBook();
                break;

            case 11:
                ReturnBook();
                break;

            case 12:
                exit = true;
                Console.WriteLine("Exiting Library Management System.");
                break;

            default:
                Console.WriteLine(
                    "Invalid option. Please choose a number from 1 to 12.");
                break;
        }
    }
    catch (Exception exception)
    {
        Console.WriteLine($"Error: {exception.Message}");
    }

    if (!exit)
    {
        Console.WriteLine();
        Console.Write("Press Enter to return to the menu...");
        Console.ReadLine();
    }
}
