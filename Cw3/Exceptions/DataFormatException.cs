using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Exceptions
{
    public class DataFormatException : Exception
    {

        public DataFormatException(string message) : base(message) { }
        
    }
}
