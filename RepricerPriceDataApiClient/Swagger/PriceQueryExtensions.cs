// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RepricerPriceDataApiClient
{
    using RepricerPriceDataApiClientTypes.Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for PriceQuery.
    /// </summary>
    public static partial class PriceQueryExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static SubmitAsinsForQueryResponse SubmitAsinListToQueue(this IPriceQuery operations, SubmitAsinsForQueryRequest request)
            {
                return operations.SubmitAsinListToQueueAsync(request).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<SubmitAsinsForQueryResponse> SubmitAsinListToQueueAsync(this IPriceQuery operations, SubmitAsinsForQueryRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SubmitAsinListToQueueWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static GetPriceQueryResultResponse GetPriceQueryResult(this IPriceQuery operations, GetPriceQueryResultRequest request)
            {
                return operations.GetPriceQueryResultAsync(request).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GetPriceQueryResultResponse> GetPriceQueryResultAsync(this IPriceQuery operations, GetPriceQueryResultRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPriceQueryResultWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}