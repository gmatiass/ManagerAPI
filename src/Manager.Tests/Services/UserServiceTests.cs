using AutoMapper;
using Bogus.DataSets;
using FluentAssertions;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Providers.Hash;
using Manager.Services.Services;
using Manager.Tests.Configuration;
using Manager.Tests.Fixtures;
using Moq;
using System;
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

            //Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }

        [Fact(DisplayName = "Create When User Exists")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserExists_ThrowsNewDomainException()
        {
            //Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();
            var userExists = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => userExists);

            //Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Create(userToCreate);
            };

            //Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Email already used.");

        }

        [Fact(DisplayName = "Create When User is Invalid")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserIsInvalid_ThrowsNewDomainException()
        {
            //Arrange
            var userToCreate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Create(userToCreate);
            };

            //Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Invalid fields.");
        }

        #endregion Create

        #region Update

        [Fact(DisplayName = "Update Valid User")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserIsValid_ReturnsUserDTO()
        {
            //Arange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateValidUserDTO();

            var hashedPassword = new Lorem().Sentence();
            var saltPassword = new Lorem().Sentence();

            var userUpdated = _mapper.Map<User>(userToUpdate);
            userUpdated.ChangePassword(hashedPassword);
            userUpdated.ChangePasswordSalt(saltPassword);            

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => oldUser);

            _hashProviderMock.Setup(x => x.GenerateHash(It.IsAny<string>()))
                .Returns(new PayloadModel
                {
                    Salt = saltPassword,
                    Hash = hashedPassword
                });

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(() => userUpdated);

            //Act
            var result = await _sut.Update(userToUpdate);

            //Arrange
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userUpdated));
        }

        [Fact(DisplayName ="Update When User Doesn't Exists")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserDoesntExists_ThrowsNewDomainException()
        {
            //Arrange
            var userToUpdate = UserFixture.CreateValidUserDTO();

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            //Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Update(userToUpdate);
            };
            
            //Arrange
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("User does not exist.");

        }

        //[Fact(DisplayName = "Update When User is Invalid")]
        //[Trait("Category", "Services")]
        //public async Task Update_WhenUserIsInvalid_ThrowsNewDomainException()
        //{
        //    //Arrange
        //    var userToUpdate = UserFixture.CreateInvalidUserDTO();
        //}

        #endregion Update

    }
}
