using Manager.Services.DTO;
using System;

namespace Manager.Services.Providers.Token
{
    public class AuthModel
    {

        public UserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }

        public AuthModel(){}

        public AuthModel(UserDTO user, string token, DateTime tokenExpires)
        {
            User = user;
            Token = token;
            TokenExpires = tokenExpires;
        }
    }
}