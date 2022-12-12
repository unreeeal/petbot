using Domain.Models;
using Newtonsoft.Json.Linq;
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

        public GoogleSheetDAO(string scriptId)
        {
            _scriptUrl= $"https://script.google.com/macros/s/{_scriptUrl}/exec";
        }

        public string GetExpensesOverview(int daysFromNow)
        {

            var queryParams = new NameValueCollection();
            queryParams.Add("daysFromNow", daysFromNow.ToString());
            var json = MakeRequest(queryParams);

            var arr = JArray.Parse(json);
            var list = new List<ExpenseModel>(arr.Count);
            foreach (var line in arr)
            {
                var mod = line.ToObject<ExpenseModel>();
                //mod.Date = DateTime.Parse(line["Date"].ToString());
                //mod.Category = line["Name"].ToString();
                //mod.Price = double.Parse(line["Price"].ToString(), CultureInfo.InvariantCulture);
                //mod.Currency = line["Currency"].ToString();
                list.Add(mod);


            }

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
            return MakeRequest(queryParams);
        }


 


        private string MakeRequest(NameValueCollection queryParams)
        {
            //a trick to get inner http collection with needed ToString method,
            //Can't call queryParams.ToString() directly it returns...
            var valCollection = HttpUtility.ParseQueryString(string.Empty);
            valCollection.Add(queryParams);
            var url = _scriptUrl + "?" + valCollection.ToString();


            using (WebClient wb = new WebClient())
            {

                for (int i = 0; i < REQUEST_TRIES; i++)
                {
                    try
                    {
                        return wb.DownloadString(url);


                    }
                    catch (WebException)
                    {


                    } 
                }

                return "Can't connect to the server";

            }

        }


    }
}
