using LibraryManagementSystem.Models;
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
}