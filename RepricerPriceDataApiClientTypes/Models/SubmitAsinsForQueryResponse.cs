// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RepricerPriceDataApiClientTypes.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SubmitAsinsForQueryResponse
    {
        /// <summary>
        /// Initializes a new instance of the SubmitAsinsForQueryResponse
        /// class.
        /// </summary>
        public SubmitAsinsForQueryResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SubmitAsinsForQueryResponse
        /// class.
        /// </summary>
        public SubmitAsinsForQueryResponse(bool isSuccessful, int errorCode, string token = default(string), string errorMsg = default(string))
        {
            Token = token;
            IsSuccessful = isSuccessful;
            ErrorMsg = errorMsg;
            ErrorCode = errorCode;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Token")]
        public string Token { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsSuccessful")]
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ErrorMsg")]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ErrorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
