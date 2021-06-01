using System.ComponentModel;

namespace Domain.Api
{
    public enum ApiValidationType
    {
        [Description("Not found")]
        NotFound,
        [Description("Already exists")]
        AlreadyExist,
        [Description("Invalid")]
        Invalid
    }
}
