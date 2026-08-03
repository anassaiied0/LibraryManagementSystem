using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _repository;
        private readonly IMemberService _memberService;
        private readonly IBookService _bookService;

        public BorrowingService(IBorrowingRepository repository, IMemberService memberService, IBookService bookService)
        {
            _repository = repository;
            _memberService = memberService;
            _bookService = bookService;
        }

        public BorrowRecord BorrowBook(int memberId, int bookId)
        {
            var member = _memberService.Search(memberId)
                ?? throw new NotFoundException(nameof(Member), memberId);

            var book = _bookService.SearchById(bookId)
                ?? throw new NotFoundException(nameof(Book), bookId);

            if (!book.IsAvailable)
                throw new ConflictException($"'{book.Title}' is already borrowed by someone else.");

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
            var book = _bookService.SearchById(bookId)
                ?? throw new NotFoundException(nameof(Book), bookId);

            var borrowRecord = _repository.GetActiveByBookId(bookId)
                ?? throw new ConflictException($"'{book.Title}' is not currently borrowed.");

            borrowRecord.ReturnDate = DateTime.Now;
            _repository.Update(borrowRecord);

            book.IsAvailable = true;
            _bookService.Update(book);

            return borrowRecord;
        }

        public List<BorrowRecord> GetBorrowRecords() => _repository.GetAll();
    }
}