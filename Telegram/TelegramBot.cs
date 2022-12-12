
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Telegram.TelegramModels;

namespace Telegram
{
    public class TelegramBot
    {

        private readonly  ILogger _logger = LogManager.GetCurrentClassLogger();
        private const int REQUEST_TRIES = 3;
        private const int MESSAGE_MAX_LENGHT = 4095;
        private readonly string _apiToken;

        /// <summary>
        /// Creates an instance of telegram bot
        /// </summary>
        /// <param name="token">api token</param>
        public TelegramBot(string token)
        {
            _apiToken = token;
        }

        /// <summary>
        /// Test function to get your name
        /// </summary>
        /// <returns>getMe instance name etc.</returns>
        public GetMeModel GetMe()
        {
            var res = MakeRequest(TelegramMethod.GetMe);
            if (!string.IsNullOrEmpty(res))
                return JObject.Parse(res)["result"]?.ToObject<GetMeModel>();
            return null;

        }
        /// <summary>
        /// Gets new updates(messages)
        /// </summary>
        /// <returns></returns>
        public List<UpdateModel> GetUpdates()
        {
            var param = new NameValueCollection
            {
                { "offset", Properties.Default.Offset.ToString() }
            };
            var res = MakeRequest(TelegramMethod.GetUpdates, param);
            if (res == null)
                return null;

            var updatesArr = JObject.Parse(res)["result"];
            var list = new List<UpdateModel>(updatesArr.Count());
            foreach (var line in updatesArr)
            {
                if (line["message"] != null)
                {

                    UpdateModel mod = line.ToObject<UpdateModel>();
                    list.Add(mod);
                }


            }

            return list;
        }


        public void SaveOffset(int offset)
        {
            if (offset != 0)
            {

                Properties.Default.Offset = offset + 1;
            }
        }

        /// <summary>
        /// Sends an image
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="file">file path</param>
        public void UploadPhoto(string chatId, string file)
        {
            UploadRequest(TelegramMethod.SendPhoto, chatId, file, "photo");
        }
        /// <summary>
        /// Sends a file
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="file">file path</param>
        public void UploadDocument(string chatId, string file)
        {
            UploadRequest(TelegramMethod.SendDocument, chatId, file, "document");
        }


        private string CreateURL(TelegramMethod method) => $"https://api.telegram.org/bot{_apiToken}/{method.Value}";

        private string UploadRequest(TelegramMethod method, string chatId, string file, string fileType = null, string reply_markup = null)
        {

            MultipartForm form = new MultipartForm(CreateURL(method));
            if (fileType != null)
                form.FileType = fileType;
            form.SetField("chat_id", chatId);
            if (reply_markup != null)
                form.SetField("reply_markup", reply_markup);
            form.SendFile(file);

            return form.ResponseText.ToString();
        }

        private string MakeRequest(TelegramMethod method, NameValueCollection pams = null)
        {

            var url = CreateURL(method);
            using (WebClient web = new WebClient())
            {
                for (int i = 0; i < REQUEST_TRIES; i++)
                {
                    try
                    {
                        if (pams == null)
                            return web.DownloadString(url);
                        byte[] responsebytes = web.UploadValues(url, "POST", pams);
                        return Encoding.UTF8.GetString(responsebytes);

                    }
                    catch (WebException ex)
                    {

                        HttpStatusCode? status = (ex.Response as HttpWebResponse)?.StatusCode;

                        if (status == HttpStatusCode.TooManyRequests || status == HttpStatusCode.BadGateway)
                        {
                            Thread.Sleep(10000);

                        }
                        else
                        {
                            var valCollection = HttpUtility.ParseQueryString(string.Empty);
                            valCollection.Add(pams);

                            _logger.Error(ex, $"query: {valCollection} method {method.Value}");
                            Thread.Sleep(1000);
                        }

                    }

                }

                return null;
            }
        }




        /// <summary>
        /// Sends text message
        /// </summary>
        /// <param name="chatId">chatId</param>
        /// <param name="message">text</param>
        public void SendMessage(string chatId, string message)
        {

            SendOrEdit(chatId, message, false, null);
            //var param = new NameValueCollection();
            //param.Add("chat_id", chatId);
            //param.Add("text", message);
            //MakeRequest(TelegramMethod.SendMessage, param);
        }


             

        private void SendOrEdit(string chatId, string text, bool disableLinkPreview, string editMessageId)
        {
            var paramsCollection = new NameValueCollection();
            if (text != null)
            {
                if (text.Length > MESSAGE_MAX_LENGHT)
                {
                    text = text.Substring(0, MESSAGE_MAX_LENGHT - 3) + "...";

                }
                paramsCollection.Add("text", text);
            }

            paramsCollection.Add("chat_id", chatId);
            if (disableLinkPreview)
                paramsCollection.Add("disable_web_page_preview", "true");

            TelegramMethod method;
            if (editMessageId != null)
            {
                paramsCollection.Add("message_id", editMessageId);
                method = TelegramMethod.EditMessageText;
            }
            else method = TelegramMethod.SendMessage;

            MakeRequest(method, paramsCollection);
        }

        /// <summary>
        /// Sends multiple images
        /// </summary>
        /// <param name="images">list of images</param>
        /// <param name="chatId">chatId</param>
        public void SendMediaGroup(List<string> images, string chatId)
        {
            var paramsCollection = new NameValueCollection();
            paramsCollection.Add("chat_id", chatId);
            Func<string, string> makeArrayRow = (link) => $@"{{""type"":""photo"",""media"":""{link}""}}";

            var innerJson = string.Join(",", images.Select(x => makeArrayRow(x)));

            paramsCollection.Add("media", $"[{innerJson}]");
            MakeRequest(TelegramMethod.SendMediaGroup, paramsCollection);
        }

        /// <summary>
        /// Creates a pop-up
        /// </summary>
        /// <param name="id">callback id</param>
        /// <param name="text">message text</param>
        public void AnswerCallback(string id, string text)
        {
            var param = new NameValueCollection();


            param.Add("callback_query_id", id);

            param.Add("text", text);
            //param.Add("show_alert", "true");
            MakeRequest(TelegramMethod.AnswerCallbackQuery, param);

        }

        /// <summary>
        /// Deletes message by messageId and chatId
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="messageId"></param>
        /// <param name="callbackId"></param>
        public void DeleteMessage(string chatId, string messageId, string callbackId = null)
        {
            var param = new NameValueCollection();
            param.Add("chat_id", chatId);
            param.Add("message_id", messageId);
            var res = MakeRequest(TelegramMethod.DeleteMessage, param);
            if (callbackId != null)
            {
                var msg = res.Length > 0 ? "deleted" : "can't delete";
                AnswerCallback(callbackId, msg);
            }

        }













    }
}
