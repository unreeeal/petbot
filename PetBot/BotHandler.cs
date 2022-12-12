using NLog;
using PetBot.CommandHandlers;
using PetBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram;

namespace PetBot
{
    public class BotHandler
    {
        private const string DEFAULT_ERROR_REPLY_MESSAGE = "error";
        private const string NO_RIGHTS_ERROR__MESSAGE = "no rights";
        private readonly TelegramBot _bot;
        private readonly TimerCallback _callbackTimerForUpdates;
        private Timer _timerForUpdates;

        private Dictionary<string, Spyer> _dictionaryOfSpyers;
        private List<CommandModel> _listOfCommands;
        private readonly string _ownerId;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();


        public BotHandler()
        {
            _bot = new TelegramBot(Config.TelegramApiToken);
            _ownerId = Config.TelegramBotOwnerId;
            _callbackTimerForUpdates = new TimerCallback(GetUpdatesCallback);
        }




        public void Start()
        {

            CreateCommandsList();
            _timerForUpdates = new Timer(_callbackTimerForUpdates, null, 0, Timeout.Infinite);
        }




        private void CreateCommandsList()
        {

            _listOfCommands = new List<CommandModel>();

            void addCommandWithReturn(string regex, Func<string> func, UserAccessLevel accessLevel = UserAccessLevel.Admin)
                => _listOfCommands.Add(new CommandModel(regex, func, accessLevel));

            void addCommandWithParamAndReturn(string regex, Func<string, string> func, UserAccessLevel accessLevel = UserAccessLevel.Admin)
                => _listOfCommands.Add(new CommandModel(regex, func, accessLevel));

            addCommandWithParamAndReturn("^/(moex|mir)", (text) => CurrencyHandler.ParseCommand(text));
            addCommandWithParamAndReturn("^/(exe|expense)", (text) =>ExpensesHandler.ParseCommand(text));
            addCommandWithParamAndReturn("^/(gas|gaz)", (text) => GazpromHanlder.ParseCommand(text));
            addCommandWithParamAndReturn("^/(spyer stop .*)", (text) => StopSpyer(Regex.Match(text, "(?<=spyer\\sstop\\s).*").Value));
            addCommandWithReturn("^/spyer all", new Func<string>(() => string.Join(Environment.NewLine, _dictionaryOfSpyers.Select(x => x.Key))));

            //get all comands regex
            addCommandWithReturn("^/all", new Func<string>(() => string.Join(Environment.NewLine, _listOfCommands.Select(x => x.Regex))));


            addCommandWithReturn(".*", () => "unknown command");

        }

        private void GetUpdatesCallback(object obj = null)
        {

            var updates = _bot.GetUpdates();
            if (updates != null)
                foreach (var line in updates)
                {
                    var chatId = line.Message.Chat.Id;
                    var text = line.Message.Text;
                    if (string.IsNullOrEmpty(text))
                        _bot.SendMessage(chatId, DEFAULT_ERROR_REPLY_MESSAGE);


                    else
                    {

                        foreach (var command in _listOfCommands)
                        {
                            if (Regex.IsMatch(text, command.Regex, RegexOptions.IgnoreCase))
                            {
                              
                                //Only owner allowed at the moment
                                if (_ownerId != chatId)
                                {
                                    _bot.SendMessage(chatId, NO_RIGHTS_ERROR__MESSAGE);
                                }

                                else if (command.Delegate is Action)
                                {
                                    command.Delegate.DynamicInvoke();
                                }
                                else if (command.Delegate is Func<string>)
                                {
                                    _bot.SendMessage(chatId, command.Delegate.DynamicInvoke()?.ToString());

                                }
                                else if (command.Delegate is Func<string, string>)
                                {
                                    _bot.SendMessage(chatId, command.Delegate.DynamicInvoke(text)?.ToString());

                                }

                                break;
                            }
                        }

                    }
                    _bot.SaveOffset(line.UpdateId);
                }
            else
            {
                _logger.Error("Can't get updates");
            }



            _timerForUpdates.Change(1000, Timeout.Infinite);
        }

        /// <summary>
        /// Adds spyer/watcher to the dictinary and runs it
        /// </summary>
        /// <param name="timeSpan">interval</param>
        /// <param name="func">function to run</param>
        /// <param name="name">any name to delete or stop spyer in the future</param>
        public void AddSpyer(TimeSpan timeSpan, Func<string> func, string name)
        {
            var spayer = new Spyer(timeSpan, func);
            AddSpyer(spayer, name);

        }
        /// <summary>
        /// Adds spyer/watcher to the dictinary and runs it
        /// </summary>
        /// <param name="spyer">Spyer instance</param>
        /// <param name="name">any name to delete or stop spyer in the future</param>
        public void AddSpyer(Spyer spyer, string name)
        {

            spyer.OnReceivedData += (s) => _bot.SendMessage(_ownerId, s);
            spyer.Start();

            if (_dictionaryOfSpyers == null)
                _dictionaryOfSpyers = new Dictionary<string, Spyer>();
            _dictionaryOfSpyers.Add(name, spyer);

        }

        private string StopSpyer(string name)
        {
            if (_dictionaryOfSpyers.ContainsKey(name))
            {
                _dictionaryOfSpyers[name].Stop();
                return "success";
            }
            return "can't stop it";
        }


    }
}
