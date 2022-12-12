
namespace Data.Gazprom.Models
{
 public   class GazpromOverviewModel
    {
        public string StationName { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{StationName} -> {Price:N2}руб.";
        }
    }
}
