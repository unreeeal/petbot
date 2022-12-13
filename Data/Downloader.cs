using NLog;
using System.Net;
using System.Text;
using System.Threading;


namespace Data
{
    public class Downloader
    {

        private const int REQUEST_TRIES = 3;

        static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Makes Get request and returns pagesource
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="headers">Web headers collection, user-agent, cookies etc.</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Pagesource</returns>
        public static string GetRequest(string url, WebHeaderCollection headers = null, Encoding encoding = null)
        {
            return MakeRequest(true, url, null, headers, encoding);

        }

        /// <summary>
        /// Makes Get request and returns pagesource
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">Data to upload</param>
        /// <param name="headers">Web headers collection, user-agent, cookies etc.</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Pagesource</returns>
        public static string PostRequest(string url, string data, WebHeaderCollection headers = null, Encoding encoding = null)
        {

            return MakeRequest(false, url, data, headers, encoding);
        }

        private static string MakeRequest(bool isGet, string url, string uploadData, WebHeaderCollection headers, Encoding encoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                _logger.Error("Can't get EMPTY URL");
                return null;
            }
            using (GZipClient wb = new GZipClient(headers, encoding))
            {
                for (int i = 0; i < REQUEST_TRIES; i++)
                {
                    try
                    {
                        if (isGet)
                            return wb.DownloadString(url);
                        else
                            return wb.UploadString(url, uploadData);

                    }
                    catch (WebException ex)
                    {

                        _logger.Error(ex, "Can't get this url" + url);
                        Thread.Sleep(1000);
                    }
                }
            }
            return null;
        }




    }


}

