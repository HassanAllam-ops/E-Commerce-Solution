using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exeptions
{
    public sealed class UserNotFoundException(string email) : NotFoundException($"Can't Find User With This Email : {email}, Try Register")
    {
    }
}
