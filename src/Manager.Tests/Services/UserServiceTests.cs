using AutoMapper;
using Manager.Infra.Interfaces;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Tests.Configuration;
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

        //#region Create

        //[Fact(DisplayName = "Create Valid User")]
        //[Trait("Category", "Services")]
        //public async Task Create_WhenUserIsValid_ReturnsUserDTO()
        //{
        //    //Arrange
        //    var userToCreate = UserFixture.CreateValidUserDTO();
        //}

        //#endregion Create

    }
}
