using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repositories
{
    public class MemberRepository
    {
        private List<Member> members = new List<Member>();

        public void Add(Member member)
        {
            members.Add(member);
        }

        public List<Member> GetAll()
        {
            return members;
        }

        public Member? GetById(int id)
        {
            return members.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var member = GetById(id);

            if (member != null)
                members.Remove(member);
        }

        public void Update(Member member)
        {
            var existing = GetById(member.Id);

            if (existing != null)
            {
                existing.FullName = member.FullName;
                existing.Email = member.Email;
                existing.PhoneNumber = member.PhoneNumber;
            }
        }
    }

    public class BookRepository
    {
        private List<Book> books = new List<Book>();

        public void Add(Book book)
        {
            books.Add(book);
        }

        public List<Book> GetAll()
        {
            return books;
        }

        public Book? GetById(int id)
        {
            return books.FirstOrDefault(x => x.Id == id);
        }

        public Book? GetByTitle(string title)
        {
            return books.FirstOrDefault(
                x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void Delete(int id)
        {
            var book = GetById(id);

            if (book != null)
                books.Remove(book);
        }

        public void Update(Book book)
        {
            var existing = GetById(book.Id);

            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                existing.PublicationYear = book.PublicationYear;
                existing.Category = book.Category;
                existing.IsAvailable = book.IsAvailable;
            }
        }
    }

    public class BorrowingRepository
    {
        private List<BorrowRecord> records = new List<BorrowRecord>();

        public void Add(BorrowRecord record)
        {
            records.Add(record);
        }

        public List<BorrowRecord> GetAll()
        {
            return records;
        }

        public BorrowRecord? GetActiveByBookId(int bookId)
        {
            return records.FirstOrDefault(r => r.Book.Id == bookId && r.ReturnDate == null);
        }

        public void Update(BorrowRecord record)
        {
            var existing = records.FirstOrDefault(r => r.Id == record.Id);
            if (existing != null)
            {
                existing.ReturnDate = record.ReturnDate;
            }
        }
    }
}
