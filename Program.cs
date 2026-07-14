using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

MemberService service = new MemberService();
bool exit = false;

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int value)) return value;
        Console.WriteLine("Invalid number. Please try again.");
    }
}

string ReadString(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? string.Empty;
}

void RegisterMember()
{
    int id = ReadInt("Id: ");
    string name = ReadString("Name: ");
    string email = ReadString("Email: ");
    string phone = ReadString("Phone: ");

    var member = new Member
    {
        Id = id,
        FullName = name,
        Email = email,
        PhoneNumber = phone
    };

    service.Register(member);
    Console.WriteLine("Member added.");
}

void UpdateMember()
{
    int id = ReadInt("Enter Member Id: ");
    Member? existing = service.Search(id);
    if (existing == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    existing.FullName = ReadString("Enter New Name (leave blank to keep): ") is string s && !string.IsNullOrWhiteSpace(s) ? s : existing.FullName;
    string newEmail = ReadString("Enter New Email (leave blank to keep): ");
    if (!string.IsNullOrWhiteSpace(newEmail)) existing.Email = newEmail;
    string newPhone = ReadString("Enter New Phone (leave blank to keep): ");
    if (!string.IsNullOrWhiteSpace(newPhone)) existing.PhoneNumber = newPhone;

    service.Update(existing);
    Console.WriteLine("Member updated successfully.");
}

void RemoveMember()
{
    int id = ReadInt("Enter Member Id: ");
    Member? existing = service.Search(id);
    if (existing == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    service.Delete(id);
    Console.WriteLine("Member deleted successfully.");
}

void SearchMember()
{
    int id = ReadInt("Enter Member Id: ");
    Member? m = service.Search(id);
    if (m == null)
    {
        Console.WriteLine("Member not found.");
        return;
    }

    Console.WriteLine("-------------------------");
    Console.WriteLine($"Id: {m.Id}");
    Console.WriteLine($"Name: {m.FullName}");
    Console.WriteLine($"Email: {m.Email}");
    Console.WriteLine($"Phone: {m.PhoneNumber}");
    Console.WriteLine("-------------------------");
}

void DisplayMembers()
{
    var members = service.GetMembers();
    if (members == null || members.Count == 0)
    {
        Console.WriteLine("No members found.");
        return;
    }

    foreach (var m in members)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"Id: {m.Id}");
        Console.WriteLine($"Name: {m.FullName}");
        Console.WriteLine($"Email: {m.Email}");
        Console.WriteLine($"Phone: {m.PhoneNumber}");
    }
    Console.WriteLine("-------------------------");
}

while (!exit)
{
    Console.WriteLine();
    Console.WriteLine("1. Register Member");
    Console.WriteLine("2. Update Member");
    Console.WriteLine("3. Remove Member");
    Console.WriteLine("4. Search Member");
    Console.WriteLine("5. Display Members");
    Console.WriteLine("0. Exit");

    int choice = ReadInt("Choice: ");

    switch (choice)
    {
        case 1:
            RegisterMember();
            break;
        case 2:
            UpdateMember();
            break;
        case 3:
            RemoveMember();
            break;
        case 4:
            SearchMember();
            break;
        case 5:
            DisplayMembers();
            break;
        case 0:
            exit = true;
            Console.WriteLine("Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid option. Please choose a valid menu item.");
            break;
    }
}