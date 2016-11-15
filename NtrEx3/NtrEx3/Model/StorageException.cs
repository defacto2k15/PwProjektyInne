using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3.Model
{
    public class StorageException : Exception
    {
        public StorageException()
        {
        }

        public StorageException(string message)
            : base(message)
        {
        }

        public StorageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
