using Caliburn.Micro;
using Microsoft.Win32;
using Repricer.Events;
using Repricer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MahApps.Metro.Controls.Dialogs;

namespace Repricer.ViewModels
{
    class InventoryListViewModel : Screen
    {
        private readonly IMapper _mapper;
        private readonly IDialogCoordinator _dialogCoordinator;
        private bool _isBusy;

        public BindableCollection<InventoryItem> InventoryItems { get; set; }
            = new BindableCollection<InventoryItem>();

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public InventoryListViewModel(IMapper mapper, IDialogCoordinator dialogCoordinator)
        {
            _mapper = mapper;
            _dialogCoordinator = dialogCoordinator;

            Task.Run(() => LoadItems());
        }

        public IEnumerable<IResult> ImportFBAItems()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                yield return Task.Run(() =>
                {
                    using (var reader = new StreamReader(dialog.FileName))
                    using (var csv = new CsvHelper.CsvReader(reader))
                    {
                        csv.Configuration.PrepareHeaderForMatch = (h, i) => h.ToLower().Replace("-", "").Replace(" ", "");
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.MissingFieldFound = null;

                        csv.Configuration.Delimiter = "\t";
                        var records = csv.GetRecords<FBAInventoryItem>();
                        Debug.WriteLine("Updating FBA Inventories...");
                        using (var db = new RepricerContext())
                        {
                            db.FBAInventoryItems.RemoveRange(db.FBAInventoryItems);
                            db.SaveChanges();


                            var item = db.FBAInventoryItems.AddRange(records.ToList());
                            db.SaveChanges();
                            Debug.WriteLine("FBA Inventories Updated!");
                            LoadItems();
                        }
                    }
                }).AsResult();
            }
        }

        private void LoadItems()
        {
            using (var db = new RepricerContext())
            {
                InventoryItems.Clear();
                InventoryItems.AddRange(db.FBAInventoryItems.ProjectTo<InventoryItem>(_mapper.ConfigurationProvider));

                foreach (var inventoryItem in InventoryItems)
                {
                    var item = db.ListedItems.FirstOrDefault(i => i.SellerSku == inventoryItem.Sku);
                    if (item == null)
                        continue;

                    inventoryItem.Title = item.ItemName;
                    inventoryItem.CurrentPrice = item.Price;
                    inventoryItem.Age = item.OpenDate.HasValue ? (int)(DateTime.Now - item.OpenDate.Value).TotalDays : -1;
                    //inventoryItem.ConditionType = item.ItemNote;
                }
            }
        }

        public IEnumerable<IResult> ImportActiveListing()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value && !string.IsNullOrWhiteSpace(dialog.FileName))
            {
                yield return Task.Run(async () =>
                {

                    var controller = await _dialogCoordinator.ShowProgressAsync(this, "Import Active Listing", "Please wait while importing your active listing\nThis could take some time depending on the numbers of items in your Active Listing.");
                    controller.SetIndeterminate();

                    Debug.WriteLine("Updating MF Inventories...");

                    using (var db = new RepricerContext())
                    {
                        using (var reader = new StreamReader(dialog.FileName))
                        using (var csv = new CsvHelper.CsvReader(reader))
                        {
                            csv.Configuration.RegisterClassMap<ListedItemMap>();
                            csv.Configuration.PrepareHeaderForMatch = (h, i) => h.ToLower().Replace("-", "").Replace(" ", "");
                            csv.Configuration.HeaderValidated = null;
                            csv.Configuration.MissingFieldFound = null;
                            // csv.Configuration.BadDataFound = null;
                            csv.Configuration.IgnoreQuotes = true;

                            csv.Configuration.Delimiter = "\t";

                            if (db.ListedItems.Any())
                            {
                                db.ListedItems.RemoveRange(db.ListedItems.ToList());
                                db.SaveChanges();
                            }

                            var records = csv.GetRecords<ListedItem>();
                            db.ListedItems.AddRange(records.ToList());
                            db.SaveChanges();
                            // LoadItems();
                            Debug.WriteLine("MF Inventories Updated!");
                            await controller.CloseAsync();
                        }
                    }
                }).AsResult();
            }
        }
    }
}
