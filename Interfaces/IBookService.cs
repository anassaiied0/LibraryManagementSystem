using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        void Add(Book book);
        List<Book> GetBooks();
        Book? SearchById(int id);
        Book? SearchByTitle(string title);
        void Delete(int id);
        void Update(Book book);
    }
}