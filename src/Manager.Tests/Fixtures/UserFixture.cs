using Bogus;
using Bogus.DataSets;
using Manager.Domain.Entities;
using Manager.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Tests.Fixtures
{
    public class UserFixture
    {
        public static User CreateValidUser()
        {
            return new User(
                name: new Name().FirstName(),
                email: new Internet().Email(),
                password: new Internet().Password());
        }

        public static List<User> CreateListValidUser(int length = 5)
        {
            var list = new List<User>();

            for(int i = 0; i < length; i++)
            {
                list.Add(CreateValidUser());
            } 

            return list;
        }

        public static UserDTO CreateValidUserDTO(bool idFlag = false)
        {
            return new UserDTO(
                id: idFlag ? new Randomizer().Int(0, 1000) : 0,
                name: new Name().FirstName(),
                email: new Internet().Email(),
                password: new Internet().Password());
        }
    }
}
