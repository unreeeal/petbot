using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram
{
    /// <summary>
    /// Contains Methods for telegram api
    /// </summary>
    internal class TelegramMethod
    {
        private TelegramMethod(string value) { Value = value; }
        public string Value { get; private set; }
        public static TelegramMethod GetMe => new TelegramMethod("getMe");
        public static TelegramMethod GetUpdates => new TelegramMethod("getUpdates");
        public static TelegramMethod SendMessage => new TelegramMethod("sendMessage");
        public static TelegramMethod EditMessageText => new TelegramMethod("editMessageText");
        public static TelegramMethod DeleteMessage => new TelegramMethod("deleteMessage");
        public static TelegramMethod SendPhoto => new TelegramMethod("sendPhoto");
        public static TelegramMethod SendDocument => new TelegramMethod("sendDocument");
        public static TelegramMethod SendMediaGroup => new TelegramMethod("sendMediaGroup");
        public static TelegramMethod AnswerCallbackQuery => new TelegramMethod("answerCallbackQuery");
    }
}
