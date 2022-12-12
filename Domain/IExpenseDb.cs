using Domain.Models;
namespace Domain
{
public  interface IExpenseDb
    {

        /// <summary>
        /// Saves expenses
        /// </summary>
        /// <param name="mod">ExpenseModel instance</param>
        /// <returns>text from with result of the procedure</returns>
        public string SaveExpense(ExpenseModel mod);
        /// <summary>
        /// Creates overview for the past Number of days
        /// </summary>
        /// <param name="daysFromNow">how many days from now</param>
        /// <returns>Overview of the past Number of days</returns>
        public string GetExpensesOverview(int daysFromNow);

    }
}
