using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class Client
    {

        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public decimal balance { get; set; }
        public string email { get; set; }

        public virtual List<Deposit> deposits { get; set; } = new();
        public virtual List<Withdrawal> withdrawals { get; set; } = new();


    }
}
