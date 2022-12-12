using PetBot.Enums;
using System;


namespace PetBot
{
    public class CommandModel
    {
        public string Regex { get; private set; }
        public Delegate Delegate { get; private set; }

        public UserAccessLevel RequiredAccessLevel { get; private set; }

        private CommandModel(Delegate dlgt,string regex,  UserAccessLevel accessLevel)
        {
            Regex = regex;
            Delegate= dlgt;
            RequiredAccessLevel = accessLevel;
        }

        public CommandModel(string regex, Action action, UserAccessLevel accessLevel) 
            : this(action, regex, accessLevel) { }

        public CommandModel(string regex, Func<string,string> func, UserAccessLevel accessLevel)
           : this(func, regex, accessLevel) { }

        public CommandModel(string regex, Func<string> func, UserAccessLevel accessLevel)
         : this(func, regex, accessLevel) { }

    }
}
