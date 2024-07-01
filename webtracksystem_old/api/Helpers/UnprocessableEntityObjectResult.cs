using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api
{
    public class UnprocessableEntity : ObjectResult
    {
        public UnprocessableEntity(ModelStateDictionary modelState) 
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new System.ArgumentNullException(nameof(modelState));
            }

            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}