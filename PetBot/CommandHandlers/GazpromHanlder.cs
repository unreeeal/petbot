using Data.Gazprom;

namespace PetBot.CommandHandlers
{
    public  class GazpromHanlder
    {
        /// <summary>
        /// Parses command from user
        /// </summary>
        /// <param name="cmd">Command should be like /gas 95 or 92 or just /gas</param>
        /// <returns>Overview with best prices</returns>
        public static string ParseCommand(string cmd)
        {
            var gasType = cmd.Contains("95") ? GasType.BENZIN95 : GasType.BENZIN92;
            var gas = new GazpromService(RegionCode.SaintPetersburg);
            return gas.GetOverview(gasType);
          
        }
    }
}
