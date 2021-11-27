using AutoMapper;
using Bogus.DataSets;
using FluentAssertions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Providers.Hash;
using Manager.Services.Services;
using Manager.Tests.Configuration;
using Manager.Tests.Fixtures;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Manager.Tests.Services
{
    public class UserServiceTests
    {
        //Subject Under Test
        private readonly IUserService _sut;

        //Mocks
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IHashProvider> _hashProviderMock;

        public UserServiceTests()
        {
            _mapper = AutoMapperConfiguration.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();
            _hashProviderMock = new Mock<IHashProvider>();

            _sut = new UserService(
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                hashProvider: _hashProviderMock.Object
            );
        
        }

        #region Create

        [Fact(DisplayName = "Create Valid User")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserIsValid_ReturnsUserDTO()
        {
            //Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();
            
            var hashedPassword = new Lorem().Sentence();
            var saltPassword = new Lorem().Sentence();
            
            var userCreated = _mapper.Map<User>(userToCreate);
            userCreated.ChangePassword(hashedPassword);
            userCreated.ChangePasswordSalt(saltPassword);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _hashProviderMock.Setup(x => x.GenerateHash(It.IsAny<string>()))
                .Returns(new PayloadModel
                {
                    Salt = saltPassword,
                    Hash = hashedPassword
                });

            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
                .ReturnsAsync(() => userCreated);

            //Act
            var result = await _sut.Create(userToCreate);

            System.Console.WriteLine($"userCreated: {userCreated}");
            System.Console.WriteLine($"result: {result}");

            //Assert
            result.Should().BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }

        #endregion Create

    }
}
