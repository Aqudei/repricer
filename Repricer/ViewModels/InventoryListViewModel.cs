using Caliburn.Micro;
using Repricer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.ViewModels
{
    class InventoryListViewModel : Screen
    {
        public BindableCollection<MFInventoryItem> MFInventoryItems { get; set; } = new BindableCollection<MFInventoryItem>();
        public BindableCollection<FBAInventoryItem> FBAInventoryItems { get; set; } = new BindableCollection<FBAInventoryItem>();

        public InventoryListViewModel()
        {
            Task.Run(() =>
            {
                using (var db = new RepricerContext())
                {
                    MFInventoryItems.AddRange(db.MFInventoryItems.ToList());
                    FBAInventoryItems.AddRange(db.FBAInventoryItems.ToList());
                }
            });
        }
    }
}
