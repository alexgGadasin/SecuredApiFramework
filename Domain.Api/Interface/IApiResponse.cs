using System.Net;

namespace Domain.Api
{
    public interface IApiResponse
    {
        HttpStatusCode StatusCode { get; set; }
        object Body { get; set; }
    }
}
