using AutoMapper;
using Manager.Infra.Interfaces;
using Manager.Services.Interfaces;
using System;
using Manager.Core.Exceptions;
using System.Threading.Tasks;
using Manager.Services.Providers.Hash;
using Microsoft.Extensions.Configuration;
using Manager.Services.Providers.Token;
using Manager.Services.DTO;

namespace Manager.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthService(IMapper mapper, IUserRepository userRepository, IHashProvider hashProvider, IConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _hashProvider = hashProvider;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthModel> CreateSession(string login, string password)
        {
            var user = await _userRepository.GetByEmail(login);

            if (user == null)
                throw new DomainException("Incorrect email/password.");


            var payload = new PayloadModel
            {
                Salt = user.PasswordSalt,
                Hash = user.Password
            };

            var passwordConfirm = _hashProvider.VerifyHash(payload, password);

            if (!passwordConfirm)
                throw new DomainException("Incorrect email/password.");
                        
            var token = _tokenGenerator.GenerateToken(user.Email);
            var tokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]));

            return new AuthModel
            {
                User = _mapper.Map<UserDTO>(user),
                Token = token,
                TokenExpires = tokenExpires
            };
        }
    }
}
