using System;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public void Add(Book book)
        {
            Validate(book);
            _repository.Add(book);
        }

        public List<Book> GetBooks() => _repository.GetAll();

        public Book? SearchById(int id) => _repository.GetById(id);

        public Book? SearchByTitle(string title) => _repository.GetByTitle(title);

        public void Delete(int id)
        {
            if (_repository.GetById(id) == null)
                throw new NotFoundException(nameof(Book), id);

            _repository.Delete(id);
        }

        public void Update(Book book)
        {
            if (_repository.GetById(book.Id) == null)
                throw new NotFoundException(nameof(Book), book.Id);

            Validate(book);
            _repository.Update(book);
        }

        private void Validate(Book book)
        {
            Guard.AgainstNullOrWhiteSpace(book.Title, nameof(book.Title));
            Guard.AgainstNullOrWhiteSpace(book.Author, nameof(book.Author));
            Guard.AgainstNullOrWhiteSpace(book.ISBN, nameof(book.ISBN));
            Guard.AgainstOutOfRange(book.PublicationYear, 1000, DateTime.Now.Year, nameof(book.PublicationYear));

            var existing = _repository.GetByISBN(book.ISBN);
            if (existing != null && existing.Id != book.Id)
                throw new ConflictException($"A book with ISBN '{book.ISBN}' already exists.");
        }
    }
}