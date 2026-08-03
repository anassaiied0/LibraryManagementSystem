using System;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Member member)
        {
            try
            {
                _context.Members.Add(member);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to save the member to the database.", ex);
            }
        }

        public List<Member> GetAll()
        {
            try
            {
                return _context.Members.AsNoTracking().ToList();
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException("Failed to retrieve members from the database.", ex);
            }
        }

        public Member? GetById(int id)
        {
            try
            {
                return _context.Members.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve member with id {id}.", ex);
            }
        }

        public Member? GetByEmail(string email)
        {
            try
            {
                return _context.Members.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex) when (ex is not AppException)
            {
                throw new DataAccessException($"Failed to retrieve member with email '{email}'.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var member = _context.Members.FirstOrDefault(x => x.Id == id);
                if (member != null)
                {
                    _context.Members.Remove(member);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException($"Failed to delete member with id {id}.", ex);
            }
        }

        public void Update(Member member)
        {
            try
            {
                _context.Members.Update(member);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DataAccessException("Failed to update the member in the database.", ex);
            }
        }
    }
}