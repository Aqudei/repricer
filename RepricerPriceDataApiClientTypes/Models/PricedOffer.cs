// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RepricerPriceDataApiClientTypes.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PricedOffer
    {
        /// <summary>
        /// Initializes a new instance of the PricedOffer class.
        /// </summary>
        public PricedOffer()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PricedOffer class.
        /// </summary>
        public PricedOffer(int conditionCode, int subConditionCode, double listingPrice, double shipping, double landedPrice, bool isFba, bool isBuyBox, bool isFeatured, int feedbackCount, double positiveFeedbackRating)
        {
            ConditionCode = conditionCode;
            SubConditionCode = subConditionCode;
            ListingPrice = listingPrice;
            Shipping = shipping;
            LandedPrice = landedPrice;
            IsFba = isFba;
            IsBuyBox = isBuyBox;
            IsFeatured = isFeatured;
            FeedbackCount = feedbackCount;
            PositiveFeedbackRating = positiveFeedbackRating;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ConditionCode")]
        public int ConditionCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SubConditionCode")]
        public int SubConditionCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ListingPrice")]
        public double ListingPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Shipping")]
        public double Shipping { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LandedPrice")]
        public double LandedPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsFba")]
        public bool IsFba { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsBuyBox")]
        public bool IsBuyBox { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsFeatured")]
        public bool IsFeatured { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FeedbackCount")]
        public int FeedbackCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PositiveFeedbackRating")]
        public double PositiveFeedbackRating { get; set; }

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
