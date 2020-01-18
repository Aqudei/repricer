using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepricerPriceDataApiClientTypes.Models;

namespace RepricerPriceDataApiClientInterfaces
{
    public interface IPricingQueryClient
    {
        SubmitAsinsForQueryResponse SubmitAsinListToQueue(SubmitAsinsForQueryRequest request);

        GetPriceQueryResultResponse GetPriceQueryResult(GetPriceQueryResultRequest request);
    }
}
