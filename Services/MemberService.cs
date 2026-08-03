using System;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;

namespace LibraryManagementSystem.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public void Register(Member member)
        {
            Validate(member);
            _repository.Add(member);
        }

        public List<Member> GetMembers() => _repository.GetAll();

        public Member? Search(int id) => _repository.GetById(id);

        public void Delete(int id)
        {
            if (_repository.GetById(id) == null)
                throw new NotFoundException(nameof(Member), id);

            _repository.Delete(id);
        }

        public void Update(Member member)
        {
            if (_repository.GetById(member.Id) == null)
                throw new NotFoundException(nameof(Member), member.Id);

            Validate(member);
            _repository.Update(member);
        }

        private void Validate(Member member)
        {
            Guard.AgainstNullOrWhiteSpace(member.FullName, nameof(member.FullName));
            Guard.AgainstInvalidEmail(member.Email);

            var existing = _repository.GetByEmail(member.Email);
            if (existing != null && existing.Id != member.Id)
                throw new ConflictException($"A member with email '{member.Email}' already exists.");
        }
    }
}