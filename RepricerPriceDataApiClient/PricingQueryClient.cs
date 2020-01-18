using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using RepricerPriceDataApiClientTypes.Models;
using RepricerPriceDataApiClientInterfaces;

namespace RepricerPriceDataApiClient
{
    public class PricingQueryClient : IPricingQueryClient
    {
        protected IPriceQuery ApiCaller { get; set; }

        public class WebApiDelegating : DelegatingHandler
        {

        }

        public PricingQueryClient()
        {
            var api = new RepricerPriceDataApi(new Uri(@"http://www.ast22.com/RepricerPriceDataApi"), new WebApiDelegating());
            ApiCaller = api.PriceQuery;
            //ApiCaller = new PriceQuery()
        }

        public SubmitAsinsForQueryResponse SubmitAsinListToQueue(SubmitAsinsForQueryRequest request)
        {
            var response = ApiCaller.SubmitAsinListToQueue(request);
            return response;
        }

        public GetPriceQueryResultResponse GetPriceQueryResult(GetPriceQueryResultRequest request)
        {
            var response = ApiCaller.GetPriceQueryResult(request);
            return response;
        }
    }
}
