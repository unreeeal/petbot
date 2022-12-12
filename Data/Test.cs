using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ConsoleApp
    {
        private readonly BotHandler botHandler;

        public ConsoleApp(BotHandler botHandler)
        {
            this.botHandler = botHandler;
        }

        public void main()
        {
            ...
        botHandler.handle(new CurrencyCommand("USD"));
            ...
    }
    }

    public class CurrencyCommand : ICommand
    {
        private readonly string type;
    
}

    public class BotHandler
    {
        private readonly ICurrencyServiceResolver currencyServiceResolver;

        public BotHandler(ICurrencyServiceResolver currencyServiceResolver)
        {
            this.currencyServiceResolver = currencyServiceResolver;
        }

        handle(CurrencyCommand cmd)
        {
            string cmdType = cmd.getType();

            ICurrencyService service = currencyServiceResolver.GetServiceByType(type);

            service.doSomething();
        }
    }

    public interface ICurrencyServiceResolver
    {
        ICurrencyService GetServiceByType(string type);
    }

    public class CurrencyServiceResolver : ICurrencyServiceResolver
    {
        private readonly IServiceProvider serviceProvider;

        public CurrencyServiceResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICurrencyService GetServiceByType(string type)
        {
            if (type == "USD")
                return serviceProvider.GetService<USDCurrencyService>();
            else if (type == "TENGE")
                return serviceProvider.GetService<TENGECurrencyService>();
            //... other condition
        }
    }

    public class USDCurrencyService : ICurrencyService
    {
        private readonly USDCurrencyDAO dao;

        public USDCurrencyService(USDCurrencyDAO dao)
        {
            this.dao = dao;
        }

        doSomething()
        {
            ...
        dao.getSomething();
            ...
    }
    }

    public interface ICurrencyService
    {
        void doSomething();
    }
}
