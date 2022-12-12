using PetBot.Enums;

namespace PetBot
{
    interface IBotUser
    {
        /// <summary>
        /// Gets user access level
        /// </summary>
        /// <returns>user access level</returns>
        public UserAccessLevel GetAccessLevel();

        /// <summary>
        /// Shows if a user has enough rights for the command
        /// </summary>
        /// <param name="requiredLevel">command required level </param>
        /// <returns>is user has enough rights</returns>
        public bool HasEnoughRights(UserAccessLevel requiredLevel);
    }
}
