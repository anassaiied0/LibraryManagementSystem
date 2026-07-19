using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class MemberService
    {
        private MemberRepository repository = new MemberRepository();

        public void Register(Member member)
        {
            repository.Add(member);
        }

        public List<Member> GetMembers()
        {
            return repository.GetAll();
        }

        public Member? Search(int id)
        {
            return repository.GetById(id);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void Update(Member member)
        {
            repository.Update(member);
        }
    }

    public class BookService
    {
        private BookRepository repository = new BookRepository();

        public void Add(Book book)
        {
            repository.Add(book);
        }

        public List<Book> GetBooks()
        {
            return repository.GetAll();
        }

        public Book? Search(int id)
        {
            return repository.GetById(id);
        }

        public Book? Search(string title)
        {
            return repository.GetByTitle(title);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void Update(Book book)
        {
            repository.Update(book);
        }
    }

    public class BorrowingService
    {
        private BorrowingRepository repository = new BorrowingRepository();
        private MemberService memberService;
        private BookService bookService;
        private int nextBorrowRecordId = 1;

        public BorrowingService(
            MemberService memberService,
            BookService bookService)
        {
            this.memberService = memberService;
            this.bookService = bookService;
        }

        public BorrowRecord BorrowBook(int memberId, int bookId)
        {
            Member? member = memberService.Search(memberId);

            if (member == null)
                throw new InvalidOperationException("Member not found.");

            Book? book = bookService.Search(bookId);

            if (book == null)
                throw new InvalidOperationException("Book not found.");

            if (!book.IsAvailable)
                throw new InvalidOperationException(
                    "This book is already borrowed.");

            BorrowRecord borrowRecord = new BorrowRecord
            {
                Id = nextBorrowRecordId,
                Member = member,
                Book = book,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            nextBorrowRecordId++;
            repository.Add(borrowRecord);

            book.IsAvailable = false;
            bookService.Update(book);

            return borrowRecord;
        }

        public BorrowRecord ReturnBook(int bookId)
        {
            Book? book = bookService.Search(bookId);

            if (book == null)
                throw new InvalidOperationException("Book not found.");

            BorrowRecord? borrowRecord =
                repository.GetActiveByBookId(bookId);

            if (borrowRecord == null)
                throw new InvalidOperationException(
                    "This book is not currently borrowed.");

            borrowRecord.ReturnDate = DateTime.Now;
            repository.Update(borrowRecord);

            book.IsAvailable = true;
            bookService.Update(book);

            return borrowRecord;
        }

        public List<BorrowRecord> GetBorrowRecords()
        {
            return repository.GetAll();
        }
    }
}
