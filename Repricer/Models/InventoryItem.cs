using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class InventoryItem : Caliburn.Micro.PropertyChangedBase
    {
        private string _title;
        private decimal _currentPrice;
        private string _conditionType;
        private int rank;

        public string Sku { get; set; }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string Asin { get; set; }

        public int Age { get; set; }
        public int Rank { get => rank; set => Set(ref rank, value); }

        public decimal CurrentPrice
        {
            get => _currentPrice;
            set => Set(ref _currentPrice, value);
        }

        public string ConditionType
        {
            get => _conditionType;
            set => Set(ref _conditionType, value);
        }

        public BindableCollection<PriceInfo> MFs { get; set; } = new BindableCollection<PriceInfo>();
        public BindableCollection<PriceInfo> FBAs { get; set; } = new BindableCollection<PriceInfo>();
        public BindableCollection<PriceInfo> BuyBoxes { get; set; } = new BindableCollection<PriceInfo>();

    }
}
