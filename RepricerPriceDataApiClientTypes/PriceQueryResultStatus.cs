﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepricerPriceDataApiClientTypes
{
    public enum PriceQueryResultStatus
    {
        InProcess = 1,
        Completed = 2,
        Error = 98,
        Rejected = 99
    }
}

