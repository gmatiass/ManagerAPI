using AutoMapper;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;

        public UserService(IMapper mapper, IUserRepository userRepository, IHashProvider hashProvider)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _hashProvider = hashProvider;
        }

        public async Task<UserDTO> Create(UserDTO userDTO)
        {
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if (userExists != null) 
                throw new DomainException("Email already used.");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var payload = _hashProvider.GenerateHash(user.Password);

            user.ChangePassword(payload.Hash);
            user.ChangePasswordSalt(payload.Salt);
            
            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
            
        }

        public async Task<UserDTO> Update(UserDTO userDTO) 
        {
            //REFACTOR: Refatorar para confirmação de password.
            var userExists = await _userRepository.Get(userDTO.Id);
            
            if (userExists == null)
                throw new DomainException("User does not exist.");

            var emailExists = await _userRepository.GetByEmail(userDTO.Email);

            if (emailExists != null && emailExists.Id != userDTO.Id)
                throw new DomainException("Email already used.");           

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var payload = _hashProvider.GenerateHash(user.Password);

            user.ChangePassword(payload.Hash);
            user.ChangePasswordSalt(payload.Salt);

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }

        public async Task Remove(long id) 
        {
            var user = await _userRepository.Remove(id);
            
            if (user == null)
                throw new DomainException("User not found.");
        }

        public async Task<UserDTO> Get(long id) 
        {
            var user = await _userRepository.Get(id);

            if (user == null)
                throw new DomainException("User not found.");

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> Get() 
        {
            var users = await _userRepository.Get();

            if (users == null)
                throw new DomainException("No users found.");

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByEmail(string email) 
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                throw new DomainException("User not found.");

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<List<UserDTO>> SearchByEmail(string email) 
        {
            var users = await _userRepository.SearchByEmail(email);

            if (users == null)
                throw new DomainException("No users found.");

            return _mapper.Map<List<UserDTO>>(users);
        }
        public async Task<List<UserDTO>> SearchByName(string name)
        {
            var users = await _userRepository.SearchByName(name);

            if (users == null)
                throw new DomainException("No users found.");

            return _mapper.Map<List<UserDTO>>(users);
        }
    }
}
