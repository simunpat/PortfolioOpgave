using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using PortfolioOpgave.Interfaces;
using PortfolioOpgave.Models;
using PortfolioOpgave.DTOs;

namespace PortfolioOpgave.Services
{
    public class UserService : IUserService, IService<User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // IService<User> implementation
        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return _userRepository.Find(predicate);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void Add(User entity)
        {
            _userRepository.Add(entity);
        }

        public void Update(User entity)
        {
            _userRepository.Update(entity);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        // IUserService implementation
        public IEnumerable<UserDto> GetAllWithDetails()
        {
            var users = _userRepository.GetAllWithDetails();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public UserDto GetByIdWithDetails(int id)
        {
            var user = _userRepository.GetByIdWithDetails(id);
            if (user == null)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        public UserDto Create(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            _userRepository.Add(user);

            return _mapper.Map<UserDto>(user);
        }

        public void Update(int id, UpdateUserDto updateUserDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            _mapper.Map(updateUserDto, user);
            _userRepository.Update(user);
        }
    }
}