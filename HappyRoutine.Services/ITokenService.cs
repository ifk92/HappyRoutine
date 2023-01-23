using HappyRoutine.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyRoutine.Services
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUserIdentity user);

    }
}
