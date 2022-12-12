using Data.Gazprom.Models;
using Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Gazprom
{
    public class GazpromService
    {

        private const int OVERVIEW_STATIONS_LIMIT = 10;

        private const string API_URL = "https://api.gpnbonus.ru/ios/v3/petrol/list_byn.txt";
        private readonly RegionCode _regionCode;

        /// <summary>
        ///    Initializes a new instance of the Gazpromservice
        /// </summary>
        /// <param name="code">Gazprom region code enum</param>
        public GazpromService(RegionCode code)
        {
            _regionCode = code;
        }

        private List<GazpromStationModel> GetListOfGasStations()
        {

            var json = Downloader.GetRequest(API_URL);
            if(string.IsNullOrEmpty(json))
            {
                return null;
            }

            JArray arr;

            try
            {
                arr = JArray.Parse(json);
            }
            catch (JsonReaderException ex)
            {

                ILogger logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Can't parse gazprom json");
                return null;
            }
            var list = new List<GazpromStationModel>(arr.Count);
            foreach (var jsObj in arr)
            {
                int.TryParse(jsObj["REGION"]?.ToString(), out int region);

                if (region != (int)_regionCode)
                    continue;

                GazpromStationModel mod = jsObj.ToObject<GazpromStationModel>();
                list.Add(mod);
            }
            return list;
        }


        /// <summary>
        /// Creates string overview with best prices, station limit OVERVIEW_STATIONS_LIMIT
        /// </summary>
        /// <param name="gasType"></param>
        /// <returns>overview with best</returns>
        public string GetOverview(string gasType)
        {
            var stations = GetListOfGasStations();
            if (stations == null)
                return "error no stations";
            var list = new List<GazpromOverviewModel>(stations.Count);
            foreach (var station in stations)
            {
                var mod = station.GasPriceArray.FirstOrDefault(x => x.Name == gasType);
                if (mod != null)
                {
                    if (mod.Price > 0)
                        list.Add(new GazpromOverviewModel
                        {
                            Price = mod.Price,
                            StationName = station.Place
                        });
                }
            }

            return string.Join(Environment.NewLine, list.OrderBy(x => x.Price).Take(OVERVIEW_STATIONS_LIMIT));
        }



    }





}
