namespace Domain.Api
{
    public interface IApiResult
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        object Data { get; set; }
    }
}
