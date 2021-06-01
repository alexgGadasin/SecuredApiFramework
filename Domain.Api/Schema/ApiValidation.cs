using System.Text.Json.Serialization;

namespace Domain.Api
{
    public class ApiValidation
    {
        public string Key { get; set; }
        public string Validation
        {
            get
            {
                return ApiHelper.GetEnumDescriptionAttribute<ApiValidationType>(ValidationType).Description;
            }
            set
            {
                ValidationType = (ApiValidationType)int.Parse(value);
            }
        }
        [JsonIgnore]
        public ApiValidationType ValidationType { get; set; }
    }
}
