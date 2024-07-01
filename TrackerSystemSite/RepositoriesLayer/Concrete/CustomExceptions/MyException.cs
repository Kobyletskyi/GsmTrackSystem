using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.CustomExceptions
{
    public class MyException : Exception
    {
        #region Constructors
        public MyException()
            : base("Unknown error.")
        {
            LocalizedMessageKey = "UnknownError";
        }
        public MyException(Exception innerException)
            : base(innerException.Message, innerException)
        {
            LocalizedMessageKey = "UnknownError";
        }
        public MyException(string localizedMessageKey)
            : base(localizedMessageKey)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public MyException(string message, string localizedMessageKey)
            : base(message)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public MyException(string localizedMessageKey, Exception innerException)
            : base(innerException.Message, innerException)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public MyException(string message, string localizedMessageKey, Exception innerException)
            : base(message, innerException)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        #endregion

        public string LocalizedMessageKey { get; set; }
    }
}
