using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
   public class ExpenseModel
    {
        public string Category { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

    }
}
