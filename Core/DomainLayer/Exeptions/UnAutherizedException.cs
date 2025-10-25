using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exeptions
{
    public sealed class UnAutherizedException(string message = "Invalid Email or Password") : Exception(message)
    {
    }
}
