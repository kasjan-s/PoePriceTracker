using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Newtonsoft.Json;
using PoePriceTracker;
using PoePriceTracker.Models;
using System.Globalization;

namespace Poller
{
    class Program
    {
        public const string DATA_CONNETION_STRING = @"";
        public const string URI_STRING = "http://apikey:DEVELOPMENT-Indexer@api.exiletools.com/index/";

        public static string QueryStringItemByName(string name, string league, int from, int size)
        {
            // Query string, as Elasticsearch.NET is pretty basic, but it suits my needs for now
            string query = @"
            {
                ""query"" : {
                    ""filtered"" : {
                        ""filter"" : {
                            ""bool"" : {
                                ""must"" : [
                                    { ""term"" : { ""attributes.rarity"" : ""Unique"" } },
                                    { ""range"": { ""shop.chaosEquiv"" : { ""gt"" : 0 } } },
                                    { ""term"" : { ""shop.verified"" : ""yes"" } },
                                    { ""term"" : { ""info.fullName"" : """ + name + @""" } },
                                    { ""term"" : { ""attributes.league"" : """ + league + @""" } }
                                ]
                            }
                        }
                    }
                },
                ""size"" : " + size + ", \"from\" : " + from + "}";
            
            return query.Replace(System.Environment.NewLine, "");
        }

        public static Response GetItemsByName(string name, string league, int from, int size, ElasticsearchClient client)
        {
            string query = QueryStringItemByName(name, league, from, size);
            var result = client.Search<string>(query);
            Response response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Response>(result.Response);

            return response;
        }


        static void Main(string[] args)
        {
            var uri = new Uri(URI_STRING);
            var settings = new ConnectionConfiguration(uri)
                .PrettyJson(true)
                .UsePrettyResponses(true);
            var client = new ElasticsearchClient(settings);

            ItemNamesDataContext itemNamesDB = new ItemNamesDataContext(DATA_CONNETION_STRING);
            ItemsDataContext itemsDB = new ItemsDataContext(DATA_CONNETION_STRING);

            // Ordering so we prioritize items that haven't been updated in a while (or ever - so null = yesterday)
            foreach (ItemName itemName in itemNamesDB.ItemNames.OrderBy(row => row.LastChecked ?? DateTime.Now.AddDays(-1)))
            {
                string[] leagues = {
                    "Standard", "Hardcore", "Warbands", "Tempest"
                };

                foreach (string league in leagues)
                {
                    int amountOfItems;
                    int scanned = 0;
                    List<float> prices = new List<float>();

                    do
                    {
                        Response response;
                        try
                        {
                            response = GetItemsByName(itemName.Name, league, scanned, 200, client);

                            amountOfItems = response.hits.total;
                            scanned += response.hits.hits.Count();

                            foreach (Hit hit in response.hits.hits)
                            {
                                prices.Add(hit._source.shop.chaosEquiv);
                            }
                        }
                        catch (System.InvalidOperationException e)
                        {
                            // Deserializer found null value -> query resulted empty response
                            amountOfItems = 0;
                        }

                        // Just to monitor status
                        Console.WriteLine(string.Format("{0} / {1}", scanned, amountOfItems));

                        // Let's try to not get banned
                        System.Threading.Thread.Sleep(2000);
                    } while (scanned < amountOfItems);

                    prices.Sort();

                    var entry = new Item
                    {
                        Name = itemName.Name,
                        Amount = amountOfItems,
                        League = league,
                        MaxPrice = prices.Any() ? prices.Last() : 0,
                        MinPrice = prices.Any() ? prices.First() : 0,
                        MeanPrice = prices.Any() ? prices.Average() : 0,
                        MedianPrice = prices.Any() ? prices[prices.Count() / 2] : 0,
                        Timestamp = DateTime.Now
                    };

                    itemsDB.Items.InsertOnSubmit(entry);
                    Console.WriteLine(string.Format("min_price: {0}, max_price: {1}, median: {2}, mean: {3}, item: {4}", 
                        entry.MinPrice, entry.MaxPrice, entry.MedianPrice, entry.MeanPrice, itemName.Name));
                }

                itemName.LastChecked = DateTime.Now;

                try
                {
                    itemNamesDB.SubmitChanges();
                    itemsDB.SubmitChanges();
                }
                catch (Exception e)
                {
                    // DB might be busy, or being accessed by someone else and submitting may fail
                    // It happens only every few hundred submits so I decided to just keep ignore it and continue

                    continue;
                }
            }
        }
    }
}
