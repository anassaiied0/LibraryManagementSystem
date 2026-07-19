namespace LibraryManagementSystem.Models
{
    public class Member
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public int PublicationYear { get; set; }

        public string Category { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
    }

    public class BorrowRecord
    {
        public int Id { get; set; }

        public Member Member { get; set; } = null!;

        public Book Book { get; set; } = null!;

        public System.DateTime BorrowDate { get; set; }

        public System.DateTime? ReturnDate { get; set; }
    }
}
