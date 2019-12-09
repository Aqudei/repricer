using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class ListedItemMap : ClassMap<ListedItem>
    {

        class DateTimeConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(text))
                        return null;

                    var dateReturned = DateTime.ParseExact(text.Substring(0, text.LastIndexOf(" ")), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    return dateReturned;
                }
                catch (Exception e)
                {
                    throw;
                }

            }
        }

        class CharYToBooleanConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (String.IsNullOrWhiteSpace(text))
                    return null;

                return text.ToUpper() == "Y" ? true : false;
            }
        }

        public ListedItemMap()
        {
            AutoMap();
            Map(m => m.OpenDate).TypeConverter<DateTimeConverter>();
            Map(m => m.ItemIsMarketPlace).TypeConverter<CharYToBooleanConverter>();
            Map(m => m.OpenDateTimeZone).ConvertUsing(e => e["opendate"].Substring(e["opendate"].LastIndexOf(" ")).Trim());
        }
    }
}
