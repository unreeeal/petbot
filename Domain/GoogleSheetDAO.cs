using Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Domain
{
    public class GoogleSheetDAO : IExpenseDb
    {

        private readonly string _scriptUrl;
        private const int REQUEST_TRIES = 3;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public GoogleSheetDAO(string scriptId)
        {
            _scriptUrl= $"https://script.google.com/macros/s/{scriptId}/exec";
        }

        public string GetExpensesOverview(int daysFromNow)
        {

            var queryParams = new NameValueCollection();
            queryParams.Add("daysFromNow", daysFromNow.ToString());
            var json = MakeRequest(queryParams);
            JArray arr;
            try
            {
                arr=JArray.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                _logger.Error(ex, "can't parse json");
                return null;
            }
            var list = new List<ExpenseModel>(arr.Count);
            foreach (var line in arr)
            {
                var mod = line.ToObject<ExpenseModel>();
                list.Add(mod);


            }
            var untilDate = DateTime.Now.AddDays(-daysFromNow);
            list = list.Where(x => x.Date > untilDate).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var group in list.GroupBy(x => x.Category))
            {
                var sum = group.Sum(x => x.Price);
                sb.AppendLine(group.Key + " " + sum);

            }
            sb.Append($"\n--------\nTotal: {list.Sum(x => x.Price)}");


            return sb.ToString();
        }




        public string SaveExpense(ExpenseModel mod)
        {
            var queryParams = new NameValueCollection();
            queryParams.Add("action", "add");
            queryParams.Add("category", mod.Category);
            queryParams.Add("price", mod.Price.ToString());
            return MakeRequest(queryParams,true);
        }





        private string MakeRequest(NameValueCollection queryParams, bool asPost=false)
        {
            string url;
            if (asPost)
                url = _scriptUrl;
            else
            {
            //a trick to get inner http collection with needed ToString method,
            //Can't call queryParams.ToString() directly it returns...
            
                var valCollection = HttpUtility.ParseQueryString(string.Empty);
                valCollection.Add(queryParams);
            url = _scriptUrl + "?" + valCollection.ToString();
            }


            using (WebClient wb = new WebClient())
            {

                for (int i = 0; i < REQUEST_TRIES; i++)
                {
                    try
                    {
                        if(!asPost)
                        return wb.DownloadString(url);
                        byte[] responsebytes = wb.UploadValues(url, "POST", queryParams);
                        return Encoding.UTF8.GetString(responsebytes);


                    }
                    catch (WebException ex)
                    {

                        _logger.Error(ex, "Can't get this url" + url);
                    } 
                }

                return "Can't connect to the server";

            }

        }


    }
}
