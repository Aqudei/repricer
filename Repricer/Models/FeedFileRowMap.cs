using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Repricer.Models
{
    sealed class FeedFileRowMap : ClassMap<FeedFileRow>
    {
        public FeedFileRowMap()
        {
            Map(m => m.Sku).Index(0).Name("sku");
            Map(m => m.Price).Index(1).Name("price");
        }
    }
}
