using Domain.Api;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Domain.Identity.Oauth
{
    public class ClientIdentifier
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public static ClientIdentifier Get(string authorization)
        {
            try
            {
                var method = authorization.Split(' ').First();
                var token = authorization.Split(' ').Last();

                if (method != "Basic")
                    throw new Exception();
                else
                {
                    var value = ApiHelper.DecodeFromBase64String(token);
                    return new ClientIdentifier()
                    {
                        client_id = value.Split(':').First(),
                        client_secret = value.Split(':').Last()
                    };
                }
            } 
            catch (Exception)
            {
                throw new ApiException("Client Identifier", ApiValidationType.Invalid);
            }
        }
    }
}
