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
        private string _status;
        public string Sku { get; set; }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public int Age { get; set; }
        public int Rank { get; set; }

        public decimal CurrentPrice
        {
            get => _currentPrice;
            set => Set(ref _currentPrice, value);
        }

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
    }
}
