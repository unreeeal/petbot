using System;
namespace PetBot.Enums
{
    /// <summary>
    /// Access levels of bot users
    /// </summary>
    [Flags]
    public enum UserAccessLevel
    {
        Admin = 1,
        Anonymous = 2,
        Friend = 4,
        Subscriber = 8


    }
}
