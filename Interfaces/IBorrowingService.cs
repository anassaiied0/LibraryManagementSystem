using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IBorrowingService
    {
        BorrowRecord BorrowBook(int memberId, int bookId);
        BorrowRecord ReturnBook(int bookId);
        List<BorrowRecord> GetBorrowRecords();
    }
}