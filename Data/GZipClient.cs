using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class GZipClient : WebClient
    {
        private const string DEFAULT_USER_AGENT ="Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36";

        public GZipClient(WebHeaderCollection headers = null, Encoding encoding = null) :base()
        {

            if (headers == null)
                this.Headers= DefaultWebHeaders();
            else
                this.Headers = headers;
            if (encoding == null)
               this.Encoding = Encoding.UTF8;
            else
            this.Encoding = encoding;
        }

        private static WebHeaderCollection DefaultWebHeaders()
        {
            var headers = new WebHeaderCollection();
            headers.Add("user-agent", DEFAULT_USER_AGENT);
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
            return headers;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }

    }

}