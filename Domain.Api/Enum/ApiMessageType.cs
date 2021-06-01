using System.ComponentModel;

namespace Domain.Api
{
    public enum ApiMessageType
    {
        [Description("{0} executed successfully")]
        SuccessfullyExecute,
        [Description("{0} fetched successfully")]
        SuccessfullyFetch,
        [Description("{0} registered successfully")]
        SuccessfullyWrite,
        [Description("{0} updated successfully")]
        SuccessfullyUpdate,
        [Description("{0} deleted successfully")]
        SuccessfullyDelete,
        [Description("{0} failed to be executed")]
        UnuccessfullyExecute,
        [Description("{0} failed to be registered")]
        UnsuccessfullyWrite,
        [Description("{0} failed to be updated")]
        UnsuccessfullyUpdate,
        [Description("{0} failed to deleted")]
        UnsuccessfullyDelete,
        [Description("Not Authorized")]
        NotAuthorized
    }
}
