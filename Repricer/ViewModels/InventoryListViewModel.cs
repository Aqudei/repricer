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

namespace Repricer.ViewModels
{
    class InventoryListViewModel : Screen
    {
        private bool isBusy;
        private int mFItemsCount;
        private int fBAItemsCount;
        private bool importMF;
        private bool importingFBA;

        public bool ImportingtMF
        {
            get => importMF; set
            {
                Set(ref importMF, value);
                NotifyOfPropertyChange(nameof(CanImportMF));
                NotifyOfPropertyChange(nameof(IsBusy));

            }
        }
        public bool ImportingFBA
        {
            get => importingFBA; set
            {
                Set(ref importingFBA, value);
                NotifyOfPropertyChange(nameof(CanImportFBA));
                NotifyOfPropertyChange(nameof(IsBusy));
            }
        }

        public BindableCollection<MFInventoryItem> MFInventoryItem { get; set; } = new BindableCollection<MFInventoryItem>();
        public BindableCollection<FBAInventoryItem> FBAInventoryItem { get; set; } = new BindableCollection<FBAInventoryItem>();
        public bool IsBusy => ImportingtMF || ImportingFBA;
        //{
        //    get => isBusy; set
        //    {
        //        Set(ref isBusy, value);

        //    }
        //}

        public int MFItemsCount { get => mFItemsCount; set => Set(ref mFItemsCount, value); }
        public int FBAItemsCount { get => fBAItemsCount; set => Set(ref fBAItemsCount, value); }
        public InventoryListViewModel()
        {
            Task.Run(LoadItems);
        }

        private void LoadItems()
        {
            using (var db = new RepricerContext())
            {
                MFItemsCount = db.MFInventoryItems.Count();
                FBAItemsCount = db.FBAInventoryItems.Count();
                MFInventoryItem.AddRange(db.MFInventoryItems.ToList());
                FBAInventoryItem.AddRange(db.FBAInventoryItems.ToList());
            }
        }

        public bool CanImportFBA => !ImportingFBA;
        public bool CanImportMF => !ImportingtMF;
        public IEnumerable<IResult> ImportFBA()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                yield break;
            }


            yield return Task.Run(() =>
            {
                ImportingFBA = true;
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
                        FBAItemsCount = 0;

                        foreach (var record in records)
                        {
                            db.FBAInventoryItems.Add(record);
                            FBAItemsCount++;
                        }

                        db.SaveChanges();
                        Debug.WriteLine("FBA Inventories Updated!");
                        LoadItems();
                    }
                }
            }).AsResult();

            ImportingFBA = false;
        }

        public IEnumerable<IResult> ImportMF()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value || string.IsNullOrEmpty(dialog.FileName))
            {
                yield break;
            }

            yield return Task.Run(() =>
            {
                ImportingtMF = true;

                Debug.WriteLine("Updating MF Inventories...");
                using (var db = new RepricerContext())
                {
                    using (var reader = new StreamReader(dialog.FileName))
                    using (var csv = new CsvHelper.CsvReader(reader))
                    {
                        csv.Configuration.RegisterClassMap<MFInventoryMap>();
                        csv.Configuration.PrepareHeaderForMatch = (h, i) => h.ToLower().Replace("-", "").Replace(" ", "");
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.MissingFieldFound = null;
                        // csv.Configuration.BadDataFound = null;
                        csv.Configuration.IgnoreQuotes = true;

                        csv.Configuration.Delimiter = "\t";

                        if (db.MFInventoryItems.Any())
                        {
                            db.MFInventoryItems.RemoveRange(db.MFInventoryItems.ToList());
                            db.SaveChanges();
                        }

                        MFItemsCount = 0;
                        var records = csv.GetRecords<MFInventoryItem>();
                        foreach (var record in records)
                        {
                            db.MFInventoryItems.Add(record);
                            MFItemsCount++;
                        }
                        db.SaveChanges();
                        LoadItems();
                        Debug.WriteLine("MF Inventories Updated!");
                    }
                }
            }).AsResult();

            ImportingtMF = false;
        }
    }
}
