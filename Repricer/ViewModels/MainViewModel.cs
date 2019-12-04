using Caliburn.Micro;
using Microsoft.Win32;
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
    class MainViewModel : Conductor<object>.Collection.OneActive
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy; set
            {
                Set(ref isBusy, value);
                NotifyOfPropertyChange(nameof(CanImportFBA));
                NotifyOfPropertyChange(nameof(CanImportMF));
            }
        }
        public MainViewModel()
        {
            ActivateItem(IoC.Get<InventoryListViewModel>());
        }

        public bool CanImportFBA => !IsBusy;
        public bool CanImportMF => !IsBusy;
        public IEnumerable<IResult> ImportFBA()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                yield return Task.Delay(TimeSpan.FromSeconds(1)).AsResult();
            }


            yield return Task.Run(() =>
            {
                IsBusy = true;
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

                        db.FBAInventoryItems.AddRange(records.ToList());
                        db.SaveChanges();

                        Debug.WriteLine("FBA Inventories Updated!");
                        IsBusy = false;
                    }
                }
            }).AsResult();
        }

        public IEnumerable<IResult> ImportMF()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                yield return Task.Delay(1).AsResult();
            }

            yield return Task.Run(() =>
            {
                IsBusy = true;
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
                        var records = csv.GetRecords<MFInventoryItem>();

                        if (db.MFInventoryItems.Any())
                        {
                            db.MFInventoryItems.RemoveRange(db.MFInventoryItems.ToList());
                            db.SaveChanges();
                        }
                        db.MFInventoryItems.AddRange(records.ToList());
                        db.SaveChanges();
                        Debug.WriteLine("MF Inventories Updated!");
                        IsBusy = false;
                    }
                }
            }).AsResult();
        }
    }
}
