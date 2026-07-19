using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
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
}
