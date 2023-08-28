using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_HW_Lesson4.Entities
{
    public class Client
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public decimal Balance { get; set; }
        public string Email { get; set; }

        public virtual List<Deposit> Deposits { get; set; } = new();
        public virtual List<Withdrawal> Withdrawals { get; set; } = new();


    }
}
