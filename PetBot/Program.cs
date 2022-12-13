using Data.Currency;
using Data.Currency.InfoClasses;
using NLog;
using NLog.Config;
using PetBot.CommandHandlers;
using System;

namespace PetBot
{

    class Program
    {


        static void Main(string[] args)
        {



            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");


            
            var botHandler = new BotHandler();
            botHandler.Start();


            //checks if the number in stock
            var tele2Spyer = new Spyer(TimeSpan.FromDays(1), () => Data.Tele2.IsInStock("9529993311") ? "check tele2" : null);
            botHandler.AddSpyer(tele2Spyer, "tele2");

            //checks ruble dollar rate and informs in case...
            var dollarSpyer = new Spyer(TimeSpan.FromMinutes(1), () => new MoexService(MoexCurrencyInfo.USD).GetRate()>61.0 ? "ruble's falling" : null);
            botHandler.AddSpyer(dollarSpyer, "moex");


            //Sends datetime every minute
            botHandler.AddSpyer(TimeSpan.FromMinutes(1), () => DateTime.Now.ToString(), "time");




            while (true)
                Console.ReadLine();
        }
    }
}
