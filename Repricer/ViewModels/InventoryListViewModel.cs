using Caliburn.Micro;
using Microsoft.Win32;
using Repricer.Events;
using Repricer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using MahApps.Metro.Controls.Dialogs;
using RepricerPriceDataApiClientTypes.Models;
using RepricerPriceDataApiClientInterfaces;
using RepricerPriceDataApiClient;

namespace Repricer.ViewModels
{
    class InventoryListViewModel : Screen
    {
        private readonly IMapper _mapper;
        private readonly IDialogCoordinator _dialogCoordinator;
        private bool _isBusy;
        private List<PricedOffersResultWithAttribute> _priceResults = new List<PricedOffersResultWithAttribute>();

        private readonly IPricingQueryClient _client = new PricingQueryClient();

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
            Task.Run(() => PopulateOfferPrices());
        }

        public IEnumerable<IResult> GenerateFeedFile()
        {
            yield return Task.Run(() =>
            {
                var saveFileDialog = new SaveFileDialog
                {
                    DefaultExt = ".txt",
                    FileName = "FeedFile.tsv"
                };

                var result = saveFileDialog.ShowDialog();
                if (!result.HasValue || !result.Value)
                    return;

                using (var stream = new StreamWriter(saveFileDialog.FileName, false))
                {
                    var writer = new CsvWriter(stream, false);
                    writer.Configuration.RegisterClassMap<FeedFileRowMap>();
                    writer.Configuration.Delimiter = "\t";
                    writer.WriteRecords(InventoryItems.Select(item => new FeedFileRow
                    {
                        Price = (double)item.CurrentPrice,
                        Sku = item.Sku
                    }));
                    writer.Flush();
                }

                Process.Start("explorer.exe", $"{Path.GetDirectoryName(saveFileDialog.FileName)}");
            }).AsResult();
        }

        private void PopulateOfferPrices()
        {
            var auth = new UserCredentialBag
            {

                MarketplaceID = "ATVPDKIKX0DER",
                Password = "xxxx",
                SellerID = "yyyy",
                UserID = "zzzz"
            };
            var tokens = new List<string>();
            var request = new SubmitAsinsForQueryRequest();
            request.UserBag = auth;

            for (int i = 0; i < InventoryItems.Count; i = i + 20)
            {
                var items = InventoryItems.Skip(i).Take(20);
                request.Asins = new List<string>();
                foreach (var item in items)
                {
                    request.Asins.Add(item.Asin);
                }

                var response = _client.SubmitAsinListToQueue(request);
                if (response.IsSuccessful)
                {
                    tokens.Add(response.Token);
                }
            }

            foreach (var token in tokens)
            {
                var priceRequest = new GetPriceQueryResultRequest
                {
                    Token = token,
                    UserBag = auth
                };

                var priceResponse = _client.GetPriceQueryResult(priceRequest);
                if (priceResponse.IsSuccessful)
                {
                    foreach (var priceAsinResponse in priceResponse.AsinPriceResults)
                    {
                        var inventoryItem = InventoryItems.FirstOrDefault(i => i.Asin == priceAsinResponse.PriceResult.Asin);
                        if (inventoryItem != null)
                        {
                            inventoryItem.Rank = priceAsinResponse.PriceResult.SalesRankList[0].RankkBackingField;
                            foreach (var priceResultLowestUsedOffer in priceAsinResponse.PriceResult.LowestUsedOffers)
                            {
                                if (priceResultLowestUsedOffer.Offer.IsBuyBox)
                                {
                                    inventoryItem.BuyBoxes.Add(new PriceInfo
                                    {
                                        ConditionCode = priceResultLowestUsedOffer.Offer.ConditionCode,
                                        SubConditionCode = priceResultLowestUsedOffer.Offer.SubConditionCode,
                                        LandedPrice = priceResultLowestUsedOffer.Offer.LandedPrice
                                    });
                                    continue;
                                }

                                if (priceResultLowestUsedOffer.Offer.IsFba)
                                {
                                    inventoryItem.FBAs.Add(new PriceInfo
                                    {
                                        ConditionCode = priceResultLowestUsedOffer.Offer.ConditionCode,
                                        SubConditionCode = priceResultLowestUsedOffer.Offer.SubConditionCode,
                                        LandedPrice = priceResultLowestUsedOffer.Offer.LandedPrice
                                    });

                                }
                                else
                                {
                                    inventoryItem.MFs.Add(new PriceInfo
                                    {
                                        ConditionCode = priceResultLowestUsedOffer.Offer.ConditionCode,
                                        SubConditionCode = priceResultLowestUsedOffer.Offer.SubConditionCode,
                                        LandedPrice = priceResultLowestUsedOffer.Offer.LandedPrice
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<IResult> ModifyPrice(InventoryItem inventoryItem)
        {
            yield return Task.Run(async () =>
            {
                var newPrice = await _dialogCoordinator.ShowInputAsync(this, "Update Price", "Please input price.", new MetroDialogSettings
                {
                    DefaultText = inventoryItem.CurrentPrice.ToString(CultureInfo.InvariantCulture)
                });
                if (string.IsNullOrWhiteSpace(newPrice))
                    return;

                inventoryItem.CurrentPrice = Decimal.Parse(newPrice);

                using (var db = new RepricerContext())
                {
                    var inDb = db.ListedItems.FirstOrDefault(i => i.SellerSku == inventoryItem.Sku);
                    if (inDb == null)
                        return;
                    inDb.Price = inventoryItem.CurrentPrice;

                    db.Entry(inDb).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }).AsResult();
        }

        public IEnumerable<IResult> ImportFBAItems()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                yield return Task.Run(async () =>
                {

                    var controller = await _dialogCoordinator.ShowProgressAsync(this, "Importing FBA Items", "Please wait while importing your FBA items.\nThis could take some time depending on the numbers of items in your Active Listing.");
                    controller.SetIndeterminate();

                    try
                    {
                        using (var reader = new StreamReader(dialog.FileName))
                        using (var csv = new CsvHelper.CsvReader(reader))
                        {
                            csv.Configuration.PrepareHeaderForMatch =
                                (h, i) => h.ToLower().Replace("-", "").Replace(" ", "");
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
                            }

                            LoadItems();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    finally
                    {
                        await controller.CloseAsync();
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


                }
            }

            try
            {
                PopulateOfferPrices();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to Fetch data from remote service.");
                Debug.WriteLine(e);
            }
        }

        private void QueryRemote()
        {
            _priceResults.Clear();

            var authBag = new UserCredentialBag
            {
                MarketplaceID = "ATVPDKIKX0DER",
                Password = "xxxx",
                SellerID = "yyyy",
                UserID = "zzzz"
            };

            var responseTokens = new List<string>();
            var asinList = new List<string>();
            var queryRequest = new SubmitAsinsForQueryRequest();

            for (int i = 0; i < InventoryItems.Count; i = i + 100)
            {
                var items = InventoryItems.Skip(i).Take(100);
                foreach (var item in items)
                {
                    asinList.Add(item.Asin);
                }
                queryRequest.UserBag = authBag;
                queryRequest.Asins = asinList;
                var response = _client.SubmitAsinListToQueue(queryRequest);
                if (!response.IsSuccessful)
                    return;

                responseTokens.Add(response.Token);
            }

            var priceResultRequestQuery = new GetPriceQueryResultRequest();
            priceResultRequestQuery.UserBag = authBag;
            bool isWaiting = true;
            GetPriceQueryResultResponse priceResponse = null;

            foreach (var token in responseTokens)
            {
                while (isWaiting)
                {
                    priceResultRequestQuery.Token = token;
                    priceResponse = _client.GetPriceQueryResult(priceResultRequestQuery);
                    if (!priceResponse.IsSuccessful)
                        break;

                    isWaiting = priceResponse.PriceQueryResultStatusId == (int)RepricerPriceDataApiClientTypes.PriceQueryResultStatus.InProcess;
                }

                if (priceResponse == null || !priceResponse.IsSuccessful || priceResponse.PriceQueryResultStatusId != (int)RepricerPriceDataApiClientTypes.PriceQueryResultStatus.Completed)
                    break;

                foreach (var priceResult in priceResponse.AsinPriceResults)
                {
                    _priceResults.Add(priceResult.PriceResult);

                    if (priceResult.PriceResult.SalesRankList != null && priceResult.PriceResult.SalesRankList.Count > 0)
                    {
                        Debug.WriteLine(priceResult.PriceResult.Asin);
                        var displayedItem = InventoryItems.FirstOrDefault(i => i.Asin == priceResult.PriceResult.Asin);
                        if (displayedItem == null)
                            continue;
                        displayedItem.Rank = priceResult.PriceResult.SalesRankList[0].RankkBackingField;
                    }
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

                    try
                    {
                        Debug.WriteLine("Updating MF Inventories...");

                        using (var db = new RepricerContext())
                        {
                            using (var reader = new StreamReader(dialog.FileName))
                            using (var csv = new CsvHelper.CsvReader(reader))
                            {
                                csv.Configuration.RegisterClassMap<ListedItemMap>();
                                csv.Configuration.PrepareHeaderForMatch =
                                    (h, i) => h.ToLower().Replace("-", "").Replace(" ", "");
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

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    finally
                    {
                        await controller.CloseAsync();
                    }

                }).AsResult();
            }
        }
    }
}
