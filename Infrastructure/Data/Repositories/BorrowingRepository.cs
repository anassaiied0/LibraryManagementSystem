using LibraryManagementSystem.Data;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly AppDbContext _context;

        public BorrowingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(BorrowRecord record)
        {
            try
            {
                _context.BorrowRecords.Add(record);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to save the borrow record to the database.", ex);
            }
        }

        public List<BorrowRecord> GetAll()
        {
            try
            {
                return _context.BorrowRecords
                    .Include(b => b.Member)
                    .Include(b => b.Book)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve borrow records from the database.", ex);
            }
        }

        public BorrowRecord? GetActiveByBookId(int bookId)
        {
            try
            {
                return _context.BorrowRecords.FirstOrDefault(r => r.BookId == bookId && r.ReturnDate == null);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve the active borrow record for book {bookId}.", ex);
            }
        }

        public void Update(BorrowRecord record)
        {
            try
            {
                _context.BorrowRecords.Update(record);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to update the borrow record in the database.", ex);
            }
        }
    }
}