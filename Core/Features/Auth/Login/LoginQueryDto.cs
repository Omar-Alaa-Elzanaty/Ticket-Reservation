using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Features.Auth.Login
{
    public class LoginQueryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}
