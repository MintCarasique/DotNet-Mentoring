using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DigitConverterClass.Exceptions
{
    [Serializable]
    public class NotADigitException : Exception
    {
        public NotADigitException() : base()
        {

        }

        public NotADigitException(string message) : base(message)
        {
            
        }

        public NotADigitException(string message, Exception inner) : base(message, inner)
        {
            
        }

        protected NotADigitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
