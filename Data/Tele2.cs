using Newtonsoft.Json.Linq;
using System;

namespace Data
{
  public class Tele2
    {
        /// <summary>
        /// Checkes if phone number is available for sale
        /// </summary>
        /// <param name="phone">desired phone number</param>
        /// <returns>is it available</returns>
        public static bool IsInStock(string phone)
        {
            var url = $"https://spb.tele2.ru/api/shop/products/numbers/bundles/1/groups?query={phone}&exclude&siteId=siteSPB";
            
            
            var ps = Downloader.GetRequest(url);
            return ps.Contains(phone);

         
        }
    }
}
