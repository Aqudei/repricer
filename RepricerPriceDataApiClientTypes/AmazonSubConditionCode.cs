using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepricerPriceDataApiClientTypes
{
    public enum AmazonSubConditionCode
    {
        Unknown = 0,
        LikeNew = 1,
        VeryGood = 2,
        Good = 3,
        Acceptable = 4,
        CollectibleLikeNew = 5,
        CollectibleVeryGood = 6,
        CollectibleGood = 7,
        CollectibleAcceptable = 8,
        NotUsed = 9,    //This entry is essential for ASMX compatability, 
        Refurbished = 10,
        New = 11
    }


}
