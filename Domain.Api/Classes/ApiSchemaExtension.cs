using System.Collections.Generic;

namespace Domain.Api
{
    public static class ApiSchemaExtension
    {
        public static void AddMessage(this List<string> validations, ApiMessageType type, object args = null)
        {
            validations.Add(ApiHelper.GetEnumDescription(type, args));
        }
    }
}
