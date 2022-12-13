using Domain;
using Domain.Models;
using System.Globalization;

namespace PetBot.CommandHandlers
{

    public class ExpensesHandler
    {
        /// <summary>
        /// Parses expense, 
        /// </summary>
        /// <param name="cmd">command /cmd must be like /exe food 300 or /exe all 7 (number of days)</param>
        /// <returns>text result of the procedure</returns>
        public static string ParseCommand(string cmd)
        {

            var parts = cmd.Split();


            if (parts.Length == 3)
            {
                IExpenseDb db = new GoogleSheetDAO(Config.GoogleSheetsDaoScriptId);

                //cmd must be like /exe food 300 or /exe all 7 (number of days)
                //exe cafe 7777
                var category = parts[1];
                var strNumber = parts[2];
                double.TryParse(strNumber, NumberStyles.Any, CultureInfo.InvariantCulture, out double number);

                if (number != 0)
                {
                    if (category.ToLowerInvariant() != "all")
                    {


                        var mod = new ExpenseModel { Category = category, Price = number };
                        return db.SaveExpense(mod);
                    }

                    else
                    {

                        return db.GetExpensesOverview((int)number);
                    }


                }
            }

            return "error cmd must be like /exe food 300 or /exe all 7 (number of days)";
        }
    }
}
