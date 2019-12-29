using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Repricer.Wpf
{
    public class SubConditionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Unknown = 0,
            //LikeNew = 1,
            //VeryGood = 2,
            //Good = 3,
            //Acceptable = 4,
            //CollectibleLikeNew = 5,
            //CollectibleVeryGood = 6,
            //CollectibleGood = 7,
            //CollectibleAcceptable = 8,
            //NotUsed = 9,    //This entry is essential for ASMX compatability, 
            //Refurbished = 10,
            //New = 11

            try
            {
                var subCon = (int)value;
                switch (subCon)
                {
                    case 0: return "Unk";
                    case 1: return "LN";
                    case 2: return "VG";
                    case 3: return "G";
                    case 4: return "A";
                    case 5: return "CLN";
                    case 6: return "CVG";
                    case 7: return "CG";
                    case 8: return "CA";
                    case 9: return "NU";
                    case 10: return "R";
                    case 11: return "N";
                    default: return "Unk";
                }
            }
            catch (Exception)
            {
                return "Unk";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
