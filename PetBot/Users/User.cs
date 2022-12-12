using PetBot.Enums;

namespace PetBot
{
    public class User:IBotUser
    {

        public string ChatId { get; private set; }
        public User(string chatId)
        {
            ChatId = chatId;
        }
        public UserAccessLevel GetAccessLevel()
        {
   
            return UserAccessLevel.Anonymous;
        }
     
        public bool HasEnoughRights(UserAccessLevel requiredLevel)
        {
            if (GetAccessLevel() == UserAccessLevel.Admin)
                return true;

            return requiredLevel.HasFlag(GetAccessLevel());
        }

    
    }
}
