using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exeptions
{
    public sealed class ProductNotFoundException(int id) : NotFoundException($"Product With Id = {id} is Not Found")
    {
    }
}
