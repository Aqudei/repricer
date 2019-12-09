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
using AutoMapper.QueryableExtensions;

namespace Repricer.ViewModels
{
    class InventoryListViewModel : Screen
    {
        private bool _isBusy;

        public BindableCollection<InventoryItem> InventoryItems { get; set; } 
            = new BindableCollection<InventoryItem>();

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public IEnumerable<IResult> ImportFBAItems()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                yield break;
            }

            IsBusy = true;

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

                        foreach (var record in records)
                        {
                            var item = db.FBAInventoryItems.Add(record);
                        }

                        db.SaveChanges();
                        Debug.WriteLine("FBA Inventories Updated!");

                        LoadItems();
                    }
                }
            }).AsResult();

            IsBusy = false;
        }

        private void LoadItems()
        {
            using (var db = new RepricerContext())
            {

            }
        }

        public IEnumerable<IResult> ImportActiveListing()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value || string.IsNullOrEmpty(dialog.FileName))
            {
                yield break;
            }

            yield return Task.Run(() =>
            {
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
                        foreach (var record in records)
                        {
                            db.ListedItems.Add(record);
                        }
                        db.SaveChanges();
                        // LoadItems();
                        Debug.WriteLine("MF Inventories Updated!");
                    }
                }
            }).AsResult();

            IsBusy = false;
        }
    }
}
