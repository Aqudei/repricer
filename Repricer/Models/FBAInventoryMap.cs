using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class FBAInventoryMap : ClassMap<FBAInventoryItem>
    {

        public FBAInventoryMap()
        {
            AutoMap();
        }
    }
}
