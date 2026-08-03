using LibraryManagementSystem.Data;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to save the book to the database.", ex);
            }
        }

        public List<Book> GetAll()
        {
            try
            {
                return _context.Books.AsNoTracking().ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve books from the database.", ex);
            }
        }

        public Book? GetById(int id)
        {
            try
            {
                return _context.Books.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve book with id {id}.", ex);
            }
        }

        public Book? GetByTitle(string title)
        {
            try
            {
                return _context.Books.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve book with title '{title}'.", ex);
            }
        }

        public Book? GetByISBN(string isbn)
        {
            try
            {
                return _context.Books.FirstOrDefault(x => x.ISBN == isbn);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve book with ISBN '{isbn}'.", ex);
            }
        }

        public void Update(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to update the book in the database.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(x => x.Id == id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException($"Failed to delete book with id {id}.", ex);
            }
        }
    }
}