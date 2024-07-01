using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.CustomExceptions
{
    public class EntityNotFoundException : MyException
    {
        #region Constructors
        public EntityNotFoundException()
            : base("Entity not found in DB.")
        {
            LocalizedMessageKey = "EntityNotFoundError";
        }
        public EntityNotFoundException(Exception innerException)
            : base(innerException.Message, innerException)
        {
            LocalizedMessageKey = "EntityNotFoundError";
        }
        public EntityNotFoundException(string localizedMessageKey)
            : base(localizedMessageKey)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public EntityNotFoundException(string message, string localizedMessageKey)
            : base(message)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public EntityNotFoundException(string localizedMessageKey, Exception innerException)
            : base(innerException.Message, innerException)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        public EntityNotFoundException(string message, string localizedMessageKey, Exception innerException)
            : base(message, innerException)
        {
            LocalizedMessageKey = localizedMessageKey;
        }
        #endregion
    }
}
