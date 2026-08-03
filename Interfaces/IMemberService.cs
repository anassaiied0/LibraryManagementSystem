using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IMemberService
    {
        void Register(Member member);
        List<Member> GetMembers();
        Member? Search(int id);
        void Delete(int id);
        void Update(Member member);
    }
}