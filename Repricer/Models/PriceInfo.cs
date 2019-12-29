using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    public class PriceInfo
    {
        public int ConditionCode { get; set; }
        public double LandedPrice { get; set; }
        public int SubConditionCode { get; set; }
    }

    //public class OfferPrice
    //{
    //    public BindableCollection<PriceInfo> MFPrices { get; set; } = new BindableCollection<PriceInfo>();
    //    public BindableCollection<PriceInfo> FBAPrices { get; set; } = new BindableCollection<PriceInfo>();
    //    public BindableCollection<PriceInfo> BuyBoxPrices { get; set; } = new BindableCollection<PriceInfo>();

    //}
}
